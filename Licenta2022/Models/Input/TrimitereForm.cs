using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class TrimitereForm
    {
        public string Observatii { get; set; }
        public int IdProgramare { get; set; }
        public int IdPacient { get; set; }
        public int IdSpecializare { get; set; }
        public List<int> IdServicii { get; set; }
    }
}