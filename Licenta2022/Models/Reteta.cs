﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class Reteta
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime DataEmiterii { get; set; }
        
        public virtual Programare Programare { get; set; }
        public virtual ICollection<RetetaXMedicament> RetetaXMedicament { get; set; }
    }
}
