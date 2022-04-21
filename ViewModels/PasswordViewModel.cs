using SistemaBiblioteca.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CursoWebsite.ViewModels
{
    public class PasswordViewModel
    {
        [PasswordPropertyText]
        [Required(ErrorMessage = "Required {0}")]
        [DataType(DataType.Password)]
        [Display(Name = "Actual Password", Prompt = "Insert Actual password")]
        public string PasswordActual { get; set; }

        [PasswordPropertyText]
        [Required(ErrorMessage = "Required {0}")]
        [StringLength(255, MinimumLength = 5,
            ErrorMessage = "the length of the {0} must be between {2} and {1} characters")]
        [DataType(DataType.Password)]
        [PasswordValidate(MinCharacter: 8)]
        [Display(Name = "Password", Prompt = "Insert password")]
        public string Password { get; set; }

        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [Compare("Password",
            ErrorMessage = "Both password must be the same")]
        [Display(Name = "Confirm your password", Prompt = "Repeat password here")]
        public string ConfirmPassword { get; set; }
    }
}
