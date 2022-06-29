using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class ClinicaInput
    {
        public string Nume { get; set; }
        public int IdLocalitate { get; set; }
        public int IdAdresa { get; set; }
    }
}