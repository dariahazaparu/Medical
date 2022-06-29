using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class AbonamentView
    {
        public int Id { get; set; }
        public string Denumire { get; set; }
        public List<ServiciuReducereView> Servicii { get; set; }
    }

    public class ServiciuReducereView 
    {
        public string DenumireServiciu { get; set; }
        public int Procent { get;set; }
    }

}