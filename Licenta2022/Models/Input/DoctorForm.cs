using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class DoctorForm
    {
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public DateTime DataAngajarii { get; set; }
        public int IdSpecializare { get; set; }
        public int IdClinica { get; set; }
    }
}