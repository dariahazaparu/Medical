using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class DoctorProgramForm
    {
        public int IdDoctor { get; set; }

        public List<Program> Programe { get; set; }
    }

    public class Program { 
        public DateTime Data { get; set; }

        public int IdProgramTemplate { get; set;  }
    }

}