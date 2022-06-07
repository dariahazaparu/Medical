using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{

    public class ProgramareViewSpecializare
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public List<ProgramareViewDoctor> Doctori { get; set; }
    }

    public class ProgramareViewDoctor
    {
        public int Id { get; set; }
        public string Nume { get; set;  }
        public string Prenume { get; set; }
 
        public List<ProgramareViewDoctorProgram> Programe { get; set; }
    }

    public class ProgramareViewDoctorProgram
    {
        public int Id { get; set; }
        public string Config { get; set; }

        public DateTime Data { get; set; }
    }
}