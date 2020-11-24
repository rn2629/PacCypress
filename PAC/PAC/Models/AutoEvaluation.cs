using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAC.Models
{
    public class AutoEvaluation
    {
        public int id { get; set; }
        public int ptsTotal { get; set; }
        public int resultat { get; set; }
        public string commentaire { get; set; }
        public int rencontreId { get; set; }
    }
}
