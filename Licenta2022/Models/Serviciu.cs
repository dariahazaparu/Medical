using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [ForeignKey("Specializare")]
        public int IdSpecializare { get; set; }
        public virtual Specializare Specializare { get; set; }

        public virtual ICollection<ServiciuXAbonament> ServiciuXAbonamente { get; set; }
        public virtual ICollection<Trimitere> Trimiteri { get; set; }
        public virtual ICollection<Programare> Programari { get; set; }
    }
}