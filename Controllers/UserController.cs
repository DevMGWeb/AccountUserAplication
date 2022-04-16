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

namespace CursoWebsite.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

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
            LoginAccount loginAccount)
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

            EditAccount editAccount = mapper.Map<EditAccount>(model);

            //Cambie esta por funcionalidad AutoMapper 
            //ViewModels.EditAccount editAccount = new EditAccount()
            //{
            //    Username = model.Username,
            //    Name = model.Name,
            //    Email = model.Email,
            //    Phone = model.Phone,
            //    Rol = model.Rol,
            //    Files = model.Files,
            //};

            return View(editAccount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditAccount account, IFormFile upload)
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
    }
}
