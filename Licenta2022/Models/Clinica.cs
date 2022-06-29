using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class Clinica
    {
        [Key, ForeignKey("Adresa")]
        public int Id { get; set; }
        public virtual Adresa Adresa { get; set; }

        [Required]
        public string Nume { get; set; }

        public virtual ICollection<Doctor> Doctori { get; set; }
    }
}