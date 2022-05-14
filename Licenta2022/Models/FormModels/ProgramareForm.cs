using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class ProgramareForm
    {
        [Key]
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public int IdPacient { get; set; }
        public int IdDoctor { get; set; }
    }
}