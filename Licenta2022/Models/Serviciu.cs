using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class Serviciu
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Denumire { get; set; }
        public int IdSpecialitate { get; set; }

        public virtual Specialitate Specialitate { get; set; }
    }
}