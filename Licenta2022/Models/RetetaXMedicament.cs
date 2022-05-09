using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class RetetaXMedicament
    {
        [Key]
        public int Id { get; set; }
        public int IdReteta { get; set; }
        public int IdMedicament { get; set; }

        public string Doza { get; set; }

        public virtual Reteta Reteta { get; set; }
        public virtual Medicament Medicament { get; set; }
    }
}