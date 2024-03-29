﻿using CursoWebsite.Helpers;
using CursoWebsite.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CursoWebsite.Services
{
    public class UserServices
    {
        public static bool Validated { get; set; }
        public static List<string> Errores { get; set; }

        public static ClaimsPrincipal AuthenticationRegister(UserAccount account)
        {
            var identity = new ClaimsIdentity(
                       CookieAuthenticationDefaults.AuthenticationScheme,
                       ClaimTypes.Name,
                       ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, account.Id));
            identity.AddClaim(new Claim(ClaimTypes.Name, account.Name));
            identity.AddClaim(new Claim(ClaimTypes.UserData, account.Username));
            identity.AddClaim(new Claim("Clave", "Valor"));

            return new ClaimsPrincipal(identity);
        }

        public static void ValidateChangePassword(UserAccount user, string passwordActual, string passwordNuevo)
        {
            Errores = new List<string>();

            if(user.Password != Encrypt.GetSHA256(passwordActual))
            {
                Errores.Add("El password actual es incorrecto");
                Validated = false;
                return;
            }

            if (user.Password == Encrypt.GetSHA256(passwordNuevo))
            {
                Errores.Add("La contraseña no puede ser igual que la anterior");
                Validated = false;
                return;
            }

            Validated = true;
        }

        internal static void SendEmailWithToken(string urlDomain, string EmailDestino, string Token)
        {
            string EmailOrigen = "mig_24@hotmail.com";
            string Contraseña = "Lluvion04";
            string url = urlDomain+ "/Access/Recovery/?token=" + Token;
            string asunto = "Recuperacion de Contraseña";
            string body = @$"<p>Recuperacion de cuenta</p>
                        <a href='{url}'>Click para recuperar cuenta</a>";

            Helpers.Mail.SendMail(EmailOrigen, Contraseña, EmailDestino, asunto, body);
        }
    }
}
