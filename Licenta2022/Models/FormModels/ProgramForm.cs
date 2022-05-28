using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class ProgramForm
    {
        public int IdDoctor { get; set; }
        public DateTime Data { get; set; }
        public List<bool> Config { get; set; }
    }
}