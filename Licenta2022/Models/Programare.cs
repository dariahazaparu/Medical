using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class Programare
    {
        [Key]
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public bool Prezent { get; set; }

        public virtual Trimitere Trimitere { get; set; }
        public virtual Trimitere TrimitereParinte { get; set; }
        public virtual ICollection<PacientXDiagnosticXProgramare> PacientXDiagnosticXProgramare { get; set; }
        public virtual Pacient Pacient { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Reteta Reteta { get; set; }
        public virtual Factura Factura { get; set; }
        public virtual Serviciu Serviciu { get; set; }
    }
}