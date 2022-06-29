using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class ServiciuXAbonament
    {
        [Key]
        public int Id { get; set; }

        #region Abonament
        [ForeignKey("Abonament")]
        public int IdAbonament { get; set; }
        public virtual Abonament Abonament { get; set; }
        #endregion

        #region Serviciu
        [ForeignKey("Serviciu")]
        public int IdServiciu { get; set; }
        public virtual Serviciu Serviciu { get; set; }
        #endregion

        public int ProcentReducere { get; set; }

    }
}