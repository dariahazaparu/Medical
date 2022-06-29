using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Licenta2022.Models
{
    public class DoctorXProgramTemplate
    {
        [Key]
        public int Id { get; set; }

        #region Doctor
        [ForeignKey("Doctor")]
        public int IdDoctor { get; set; }
        public virtual Doctor Doctor { get; set; }
        #endregion

        #region ProgramTemplate
        [ForeignKey("ProgramTemplate")]
        public int IdProgramTemplate { get; set; }
        public virtual ProgramTemplate ProgramTemplate { get; set; }
        #endregion

        public string Config { get; set; }
        public DateTime Data { get; set; }
    }
}