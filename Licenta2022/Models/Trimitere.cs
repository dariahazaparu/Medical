using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class Trimitere
    {
        [Key]
        public int Id { get; set; }
        public string Observatii { get; set; }

        public bool org { get; set; }
        public virtual Programare Programare { get; set; } 
        public virtual Pacient Pacient { get; set; }
        public virtual Specialitate Specialitate { get; set; }

    }
}