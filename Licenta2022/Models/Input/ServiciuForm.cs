using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class ServiciuForm
    {
        public string Denumire { get; set; }
        public float Pret { get; set; }
        public int IdSpecializare { get; set; }
    }
}