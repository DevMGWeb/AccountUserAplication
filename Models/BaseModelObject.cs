using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CursoWebsite.Models
{
    public abstract class BaseModelObject
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Required {0}")]
        [StringLength(30, MinimumLength = 3, 
            ErrorMessage = "the length of the {0} must be between {2} and {1} characters")]
        [Display(Name = "Name", Prompt = "Insert your name here")]
        public virtual string Name { get; set; }

        public BaseModelObject()
        {
            this.Id = System.Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            return $"{Name},{Id}";
        }
    }
}
