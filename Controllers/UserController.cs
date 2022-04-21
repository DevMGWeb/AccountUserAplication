using CursoWebsite.Models;
using CursoWebsite.Services;
using CursoWebsite.Helpers;
using CursoWebsite.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace CursoWebsite.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private string urlDomain = "https://localhost:44345";

        public UserController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync([Bind(include: "Name, Username, Password, ConfirmPassword")]
            UserAccount account)
        {
            if (ModelState.IsValid)
            {
                account.Password = Encrypt.GetSHA256(account.Password);
                context.UserAccounts.Add(account);
                context.SaveChanges();

                await AuthenticationRegisterAsync(account);
                return RedirectToAction("Index","Main");
            }

            return View(account);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync([Bind(include: "Username, Password")] 
            LoginAccountViewModel loginAccount)
        {
            if (ModelState.IsValid)
            {
                var account = context.UserAccounts
                    .FirstOrDefault(x => x.Username == loginAccount.Username);

                if(account != null)
                {
                    if(account.Password == Encrypt.GetSHA256(loginAccount.Password))
                    {
                        await AuthenticationRegisterAsync(account);
                        return RedirectToAction("Index", "Main");
                    }

                    ModelState.AddModelError("Error", "Contraseña invalida");
                    return View("Login", loginAccount);
                }

                ModelState.AddModelError("Error", "Usuario no encontrado");
                return View("Login", loginAccount);
            }

            return View("Login", loginAccount);
        }

        public async Task<IActionResult> PassAsGuestAsync()
        {
            var account = context.UserAccounts
                   .FirstOrDefault(x => x.Username == "Guest");

            await AuthenticationRegisterAsync(account);

            return RedirectToAction("Index", "Main");
        }

        private async Task AuthenticationRegisterAsync(UserAccount account)
        {
            var principal = UserServices.AuthenticationRegister(account);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties { ExpiresUtc = DateTime.Now.AddDays(1), IsPersistent = true });
        }

        [Authorize]
        public IActionResult Edit()
        {
            string Id = Helpers.Session.GetNameIdentifier(User);
            var model = context.UserAccounts.Include(f => f.Files)
                .FirstOrDefault(x => x.Id == Id);

            EditAccountViewModel editAccount = mapper.Map<EditAccountViewModel>(model);

            return View(editAccount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditAccountViewModel account, IFormFile upload)
        {
            if (ModelState.IsValid)
            {
                string Id = Helpers.Session.GetNameIdentifier(User);
                var model = context.UserAccounts.Include(f => f.Files)
                   .FirstOrDefault(x => x.Id == Id);

                if (upload != null && upload.Length > 0)
                {
                    account.Files = FilesServices.GetFiles(upload, FileType.Avatar);
                    model.Files = account.Files;
                }
                else if(model.Files != null)
                {
                    account.Files = model.Files;
                }

                model = mapper.Map(account, model);

                context.Update(model);
                context.SaveChanges();
            }

            return View(account);
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePasswordAsync(PasswordViewModel models)
        {
            if (ModelState.IsValid)
            {
                string Id = Helpers.Session.GetNameIdentifier(User);
                var user = context.UserAccounts.FirstOrDefault(x => x.Id == Id);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                UserServices.ValidateChangePassword(user, models.PasswordActual, models.Password);

                if (!UserServices.Validated)
                {
                    foreach (var error in UserServices.Errores)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }

                    return View(models);
                }

                user.Password = Encrypt.GetSHA256(models.Password); 
                context.Update(user);
                await context.SaveChangesAsync();

                return View("ChangePasswordConfirmation");
            }

            return View(models);
        }

        [HttpGet]
        public IActionResult StartRecovery()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartRecovery(RecoveryAccountViewModel recovery)
        {
            if (!ModelState.IsValid)
            {
                return View(recovery);
            }

            string token = Encrypt.GetSHA256(System.Guid.NewGuid().ToString());

            var user = context.UserAccounts
                .FirstOrDefault(x => x.Email == recovery.Email);

            if(user != null)
            {
                user.Token_Recovery = token;
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();

                //Enviar email
                UserServices.SendEmailWithToken(urlDomain, user.Email, token);
            }

            ViewBag.SuccessMessage = "Your token has been sent successfully";
            return View();
        }

        [HttpGet]
        [Route("Access/Recovery")]
        public IActionResult Recovery(string token)
        {
            var recovery = new RecoveryPasswordAccountViewModel();
            recovery.token_key = token;

            if(token == string.Empty || token == null)
            {
                return RedirectToAction("Login");
            } 

            var oUser = context.UserAccounts.SingleOrDefault(x => x.Token_Recovery == token);
            if (oUser == null) 
            {
                ViewBag.ErrorMessage = "Sorry, Your token has expired";
                return View("_ErrorMessageView");
            }

            return View(recovery);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Access/Recovery")]
        public IActionResult Recovery(RecoveryPasswordAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var oUser = context.UserAccounts.SingleOrDefault(x => x.Token_Recovery == model.token_key);
                if(oUser != null)
                {
                    oUser.Password = Encrypt.GetSHA256(model.Password);
                    oUser.Token_Recovery = null;
                    context.Entry<UserAccount>(oUser).State = EntityState.Modified;
                    context.SaveChanges();
                    ViewBag.SuccessMessage = "Your password has been changed successfully";
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return View();
        }
    }
}
