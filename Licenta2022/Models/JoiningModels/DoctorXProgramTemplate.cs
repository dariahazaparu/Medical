using System;
using System.ComponentModel.DataAnnotations;

namespace Licenta2022.Models
{
    public class DoctorXProgramTemplate
    {
        [Key]
        public int Id { get; set; }

        public int IdDoctor { get; set; }

        public virtual Doctor Doctor { get; set; }

        public int IdProgramTemplate { get; set; }
        public virtual ProgramTemplate ProgramTemplate { get; set; }

        public string Config { get; set; }

        public DateTime Data { get; set; }
    }
}