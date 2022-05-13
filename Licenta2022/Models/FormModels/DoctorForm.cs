using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class DoctorForm
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nume { get; set; }
        [Required]
        public string Prenume { get; set; }
        public DateTime DataAngajarii { get; set; }
        public int IdSpecialitate { get; set; }
        public int IdClinica { get; set; }
    }
}