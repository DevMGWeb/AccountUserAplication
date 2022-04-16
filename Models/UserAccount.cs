using SistemaBiblioteca.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CursoWebsite.Models
{
    public class UserAccount : BaseModelObject
    {
        [Required(ErrorMessage = "Required {0}")]
        [StringLength(30, MinimumLength = 3, 
            ErrorMessage = "the length of the {0} must be between {2} and {1} characters")]
        [Display(Name = "Username", Prompt = "Insert username")]
        public string Username { get; set; }

        [PasswordPropertyText]
        [Required(ErrorMessage = "Required {0}")]
        [StringLength(255, MinimumLength = 5,
            ErrorMessage = "the length of the {0} must be between {2} and {1} characters")]
        [DataType(DataType.Password)]
        [PasswordValidate(MinCharacter: 8)]
        [Display(Name = "Password", Prompt = "Insert password")]
        public string Password { get; set; }

        [PasswordPropertyText]
        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password", 
            ErrorMessage = "Both password must be the same")]
        [Display(Name = "Confirm your password", Prompt = "Repeat password here")]
        public string ConfirmPassword { get; set; }

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
