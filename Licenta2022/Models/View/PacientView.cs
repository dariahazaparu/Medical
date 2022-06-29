using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class PacientView: IdDictionaryItem
    {
        public AdresaView Adresa  { get; set; }
        public string UserId { get; set; }
    }
    public class AdresaView
    {
        public string Localitate { get; set; }
        public string Strada { get; set; }
        public int? Numar { get; set; }
    }
}