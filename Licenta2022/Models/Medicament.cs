using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class Medicament
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Denumire { get; set; }
        public string Observatii { get; set; }

        public virtual ICollection<RetetaXMedicament> RetetaXMedicament { get; set; }
    }
}
