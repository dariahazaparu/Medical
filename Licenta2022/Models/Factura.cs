using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class Factura
    {
        [Key]
        public int Id { get; set; }
        public float Total { get; set; }
        public virtual Programare Programare { get; set; }
    }
}