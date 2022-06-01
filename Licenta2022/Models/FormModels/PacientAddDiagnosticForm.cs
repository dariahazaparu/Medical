using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class PacientAddDiagnosticForm
    {
        public int IdPacient { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public int IdDiagnostic { get; set; }
    }
}