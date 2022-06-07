using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class Pacient
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nume { get; set; }
        [Required]
        public string Prenume { get; set; }
        [Required]
        public string CNP { get; set; }

        public virtual Adresa Adresa { get; set; }
        public virtual Asigurare Asigurare { get; set; }
        public virtual ICollection<PacientXDiagnosticXProgramare> PacientXDiagnosticXProgramare { get; set; }
        public virtual ICollection<Programare> Programari { get; set; }
    }
}