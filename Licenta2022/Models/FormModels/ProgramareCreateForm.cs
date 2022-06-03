using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class ProgramareCreateForm
    {
        public int IdDoctor { get; set; }
        public int IdProgram { get; set; }
        public int ProgramIntervalIndex { get; set; }
        public int IdPacient { get; set; }
    }
}