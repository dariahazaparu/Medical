using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class Asigurare
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Denumire { get; set; }
        public List<int> IdServicii { get; set; }
        public List<int> ProcenteReducere { get; set; }

        public virtual ICollection<ServiciuXAsigurare> ServiciuXAsigurari { get; set; }
    }
}