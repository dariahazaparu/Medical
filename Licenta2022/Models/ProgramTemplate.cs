using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class ProgramTemplate
    {
        [Key]
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Config { get; set; }
        public string Descriere { get; set; }

        public virtual ICollection<Doctor> Doctori { get; set; }
    }
}