﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class PacientForm
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nume { get; set; }
        [Required]
        public string Prenume { get; set; }
        [Required]
        public string CNP { get; set; }
        public int IdAdresa { get; set; }
        public int IdAsigurare { get; set; }
    }
}