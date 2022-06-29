using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class Abonament
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Denumire { get; set; }
        public virtual ICollection<ServiciuXAbonament> ServiciuXAbonamente { get; set; }
    }
}