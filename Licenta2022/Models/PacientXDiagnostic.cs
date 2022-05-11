using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class PacientXDiagnostic
    {
        [Key]
        public int Id { get; set; }
        public int IdPacient { get; set; }
        public int IdDiagnostic { get; set; }
        public DateTime Data { get; set; }

        public virtual Diagnostic Diagnostic { get; set; }
        public virtual Pacient Pacient { get; set; }
    }
}