using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class AbonamentInput
    {
        public string Denumire { get; set; }
        public List<int> IdServicii { get; set; }
        public List<int> ProcenteReducere { get; set; }

    }
}