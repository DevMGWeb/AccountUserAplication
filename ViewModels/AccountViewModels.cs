using CursoWebsite.Models;
using SistemaBiblioteca.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CursoWebsite.ViewModels
{
    public class LoginAccountViewModel
    {
        [Required(ErrorMessage = "Required {0}")]
        [Display(Name = "Username", Prompt = "Insert username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required {0}")]
        [Display(Name = "Password", Prompt = "Insert password")]
        public string Password { get; set; }
    }

    public class EditAccountViewModel : BaseModelObject
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

    public class RecoveryAccountViewModel
    {
        [Required(ErrorMessage = "Required {0}")]
        [EmailAddress(ErrorMessage = "Format Incorrect")]
        public string Email { get; set; }
    }

    public class RecoveryPasswordAccountViewModel
    {
        [Required]
        public string token_key { get; set; }

        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 5,
          ErrorMessage = "the length of the {0} must be between {2} and {1} characters")]
        [PasswordValidate(MinCharacter: 8)]
        [Required(ErrorMessage = "Required {0}")]
        [Display(Name = "New Password", Prompt = "Put here your new password")]
        public string Password { get; set; }

        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [Compare("Password",
          ErrorMessage = "Both password must be the same")]
        [Display(Name = "Confirm Your Password", Prompt = "Repeat here your new password")]
        public string Password_Confirm { get; set; }
    }
}
