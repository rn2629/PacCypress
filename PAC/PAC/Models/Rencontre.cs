using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAC.Models
{
    public class Rencontre
    {
       public int id { get; set; }
       public string etudiantId { get; set; }
       public int seanceCoursId { get; set; }

        public Rencontre(int id, string etudiantId, int seanceCoursId)
        {
            this.id = id;
            this.etudiantId = etudiantId;
            this.seanceCoursId = seanceCoursId;
        }

        public Rencontre()
        {

        }
    }
}
