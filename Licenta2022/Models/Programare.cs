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
        [Required]
        public DateTime Data { get; set; }
        public bool Prezent { get; set; }

        public virtual Trimitere Trimitere { get; set; }

        #region TrimitereParinte
        [ForeignKey("Trimitere")]
        public int? IdTrimitereParinte { get; set; }
        public virtual Trimitere TrimitereParinte { get; set; }
        #endregion

        #region Pacient
        [ForeignKey("Pacient")]
        public int IdPacient { get; set; }
        public virtual Pacient Pacient { get; set; }
        #endregion

        #region Doctor
        [ForeignKey("Doctor")]
        public int IdDoctor { get; set; }
        public virtual Doctor Doctor { get; set; }
        #endregion

        #region Serviciu
        [ForeignKey("Serviciu")]
        public int? IdServiciu { get; set; }
        public virtual Serviciu Serviciu { get; set; }
        #endregion

        public virtual Reteta Reteta { get; set; }
        public virtual Factura Factura { get; set; }
        public virtual ICollection<PacientXDiagnosticXProgramare> PacientXDiagnosticXProgramare { get; set; }
    }
}