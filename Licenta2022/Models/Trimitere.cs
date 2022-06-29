using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class Trimitere
    {
        [Key]
        public int Id { get; set; }
        public string Observatii { get; set; }

        public virtual Programare Programare { get; set; }

        [ForeignKey("Programare")]
        public int IdProgramareParinte { get; set; }
        public virtual Programare ProgramareParinte { get; set; }

        [ForeignKey("Specializare")]
        public int IdSpecializare { get; set; }
        public virtual Specializare Specializare { get; set; }

        public virtual ICollection<Serviciu> Servicii { get; set; }

    }
}