using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class ServiciuXAsigurare
    {
        [Key]
        public int Id { get; set; }
        public int IdAsigurare { get; set; }
        public int IdServiciu { get; set; }
        public int ProcentReducere { get; set; }

        public virtual Asigurare Asigurare { get; set; }
        public virtual Serviciu Serviciu { get; set; }
    }
}