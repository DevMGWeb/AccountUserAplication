using CursoWebsite.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CursoWebsite.ViewModels
{
    public class LoginAccount
    {
        [Required(ErrorMessage = "Required {0}")]
        [Display(Name = "Username", Prompt = "Insert username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required {0}")]
        [Display(Name = "Password", Prompt = "Insert password")]
        public string Password { get; set; }
    }

    public class EditAccount : BaseModelObject
    {
        [Required(ErrorMessage = "Required {0}")]
        [StringLength(30, MinimumLength = 3,
           ErrorMessage = "the length of the {0} must be between {2} and {1} characters")]
        [Display(Name = "Username", Prompt = "Insert username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required {0}")]
        [StringLength(30, MinimumLength = 3,
          ErrorMessage = "the length of the {0} must be between {2} and {1} characters")]
        [Display(Name = "Email", Prompt = "Insert {0}")]
        [EmailAddress(ErrorMessage = "Format Incorrect")]
        public string Email { get; set; }

        [StringLength(30, MinimumLength = 3,
            ErrorMessage = "the length of the {0} must be between {2} and {1} characters")]
        [Display(Name = "Phone", Prompt = "Insert {0}")]
        [Phone(ErrorMessage = "Format Incorrect")]
        public string Phone { get; set; }

        public Rol Rol { get; set; }

        public virtual ICollection<File> Files { get; set; }
    }
}
