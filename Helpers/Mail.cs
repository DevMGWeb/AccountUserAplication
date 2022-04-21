using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CursoWebsite.Helpers
{
    public static class Mail
    {
        public static void SendMail(string EmailOrigen, string Contraseña,
            string EmailDestino, string asunto, string body)
        {
            MailMessage mailMessage = new MailMessage(EmailOrigen, EmailDestino,
              asunto, body);

            mailMessage.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.live.com");
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential(EmailOrigen, Contraseña);

            smtp.Send(mailMessage);

            smtp.Dispose();
        }
    }
}
