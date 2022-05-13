using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class RetetaForm
    {
        [Key]
        public int Id { get; set; }
        public List<int> IdMedicamente { get; set; }
        public List<string> Doze { get; set; }
    }
}