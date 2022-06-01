using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class PacientEditForm
    {
        public int Id { get; set; }

        public string Nume { get; set; }

        public string Prenume { get; set; }
        public string CNP { get; set; }
        public int IdAdresa { get; set; }
        public int IdLocalitate { get; set; }
        public int IdAsigurare { get; set; }

    }
}