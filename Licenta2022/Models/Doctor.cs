using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nume { get; set; }
        [Required]
        public string Prenume { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DataAngajarii { get; set; }

        public virtual Specializare Specializare { get; set; }
        public virtual Clinica Clinica { get; set; }

        public virtual ICollection<DoctorXProgramTemplate> DoctorXProgramTemplates { get; set; }
    }
}