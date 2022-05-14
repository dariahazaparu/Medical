using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class Specialitate
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Denumire { get; set; }

        public virtual ICollection<Doctor> Doctori { get; set; }
        public virtual ICollection<Serviciu> Servicii { get; set; }
        public virtual ICollection<Trimitere> Trimiteri { get; set; }
    }
}