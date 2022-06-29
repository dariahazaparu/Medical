using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class RetetaXMedicament
    {
        [Key]
        public int Id { get; set; }

        #region Reteta
        [ForeignKey("Reteta")]
        public int IdReteta { get; set; }
        public virtual Reteta Reteta { get; set; }
        #endregion

        #region Medicament
        [ForeignKey("Medicament")]
        public int IdMedicament { get; set; }
        public virtual Medicament Medicament { get; set; }
        #endregion

        public string Doza { get; set; }

    }
}