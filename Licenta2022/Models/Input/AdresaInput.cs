using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class AdresaInput
    {
        public string Strada { get; set; }
        public int Numar { get; set; }
        public int IdLocalitate { get; set; }

    }
}