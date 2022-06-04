using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class Factura
    {
        [Key, ForeignKey("Programare")]
        public int Id { get; set; }
        public float Total { get; set; }
        public virtual Programare Programare { get; set; }
    }
}