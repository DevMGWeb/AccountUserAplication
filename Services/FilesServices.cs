using CursoWebsite.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CursoWebsite.Services
{
    public class FilesServices
    {
        public static List<File> GetFiles(IFormFile upload, FileType fileType)
        {
            var file = new File
            {
                Name = System.IO.Path.GetFileName(upload.FileName),
                FileType = fileType,
                ContentType = upload.ContentType
            };
            using (var reader = new System.IO.BinaryReader(upload.OpenReadStream()))
            {
                file.Content = reader.ReadBytes((int)upload.Length);
            }

            return new List<File> { file };
        }
    }
}
