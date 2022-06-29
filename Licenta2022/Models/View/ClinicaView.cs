using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class ClinicaView
    {
        public int Id { get; set; }
        public string Denumire { get; set; }
        public string AdresaComplet { get; set; }    
        public List<DoctorClinicaView> Doctori { get; set; }
    }

    public class DoctorClinicaView
    {
        public string NumeComplet { get; set; } 
        public string Specializare { get; set; }    
    }
}