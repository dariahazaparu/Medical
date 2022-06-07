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
        [Required]
        public float Pret { get; set; }

        public virtual Specializare Specializare { get; set; }
        public virtual ICollection<ServiciuXAsigurare> ServiciuXAsigurari { get; set; }
        public virtual ICollection<Trimitere> Trimiteri { get; set; }
        public virtual ICollection<Programare> Programari { get; set; }
    }
}