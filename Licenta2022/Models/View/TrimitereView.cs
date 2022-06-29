using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class TrimitereView
    {
        public int Id { get; set; }
        public string Observatii { get; set; }
        public IdDictionaryItem Pacient { get; set; }
        public int IdProgramare { get; set; }
        public int IdProgramareParinte { get; set; }
        public IEnumerable<string> Servicii { get; set; }
        public string Specializare { get; set; }

    }

    public class TrimitereIndexView
    {
        public int Id { get; set; }
        public PacientUserView Pacient { get; set; }
        public DateTime DataProgramare { get; set; }
        public string Specializare { get; set; }
    }
}