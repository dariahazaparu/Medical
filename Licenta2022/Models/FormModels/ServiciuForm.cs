using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class ServiciuForm
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Denumire { get; set; }
        [Required]
        public float Pret { get; set; }
        public int IdSpecialitate { get; set; }
    }
}