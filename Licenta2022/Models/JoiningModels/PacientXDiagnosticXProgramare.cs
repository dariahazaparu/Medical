using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class PacientXDiagnosticXProgramare
    {
        [Key]
        public int Id { get; set; }

        #region Pacient
        [ForeignKey("Pacient")]
        public int IdPacient { get; set; }
        public virtual Pacient Pacient { get; set; }
        #endregion

        #region Diagnostic
        [ForeignKey("Diagnostic")]
        public int IdDiagnostic { get; set; }
        public virtual Diagnostic Diagnostic { get; set; }
        #endregion

        #region Programare
        [ForeignKey("Programare")]
        public int IdProgramare { get; set; }
        public virtual Programare Programare { get; set; }
        #endregion

        public DateTime Data { get; set; }

    }
}