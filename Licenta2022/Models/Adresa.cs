using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class Adresa
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Strada { get; set; }
        public int Numar { get; set; }
        
        public virtual ICollection<Pacient> Pacienti { get; set; }
        public virtual Clinica Clinica { get; set; }
        public virtual Localitate Localitate { get; set; }
    }
}