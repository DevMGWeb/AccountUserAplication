using CursoWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CursoWebsite.Controllers
{
    public class ManagerController : Controller
    {
        private readonly ApplicationDbContext context;

        public ManagerController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public FileResult GetFileFromBytes(byte[] bytesIn, string contentType)
        {
            return File(bytesIn, contentType);
        }

        [HttpGet]
        public IActionResult GetUserImageFile()
        {
            var user = context.UserAccounts.Include(s => s.Files)
                        .FirstOrDefault(x => x.Id == Helpers.Session.GetNameIdentifier(User));
            if (user == null)
            {
                return null;
            }

            var image = (user.Files.FirstOrDefault(x => x.FileType == FileType.Avatar));
            FileResult imageUserFile = GetFileFromBytes(image.Content, image.ContentType);
            return imageUserFile;
        }
    }
}
