using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CursoWebsite.Models
{
    public class File: BaseModelObject
    {
        [StringLength(255)]
        public override string Name { get => base.Name; set => base.Name = value; }

        [StringLength(100)]
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public FileType FileType { get; set; }
        public string UserAccountId { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
