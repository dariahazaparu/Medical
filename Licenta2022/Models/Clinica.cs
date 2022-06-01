using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class Clinica
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nume { get; set; }

        public virtual ICollection<Doctor> Doctori { get; set; }
        public virtual Adresa Adresa { get; set; }
    }
}