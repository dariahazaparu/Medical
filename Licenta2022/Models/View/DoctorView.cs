using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class DoctorView : IdDictionaryItem
    {
        public string Specializare { get; set; }
        public string Clinica { get; set; } 
        public List<ProgramDoctorView> Configuratii { get; set; }
    }

    public class ProgramDoctorView
    {
        public string Config { get; set; }  
        public DateTime Data { get; set; }
    }
}