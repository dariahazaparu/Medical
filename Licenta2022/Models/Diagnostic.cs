using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class Diagnostic
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Denumire { get; set; }
        [Required]
        public string Descriere { get; set; }

        public virtual ICollection<PacientXDiagnostic> PacientXDiagnostics { get; set; }
    }
}