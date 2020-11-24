using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PAC.Models
{
    public class RencontreModel
    {
        public List<IdentityUser> listeEtudiant{ get;set; }
        public Rencontre rencontre { get;set; }

        public DatePickerEvent cours { get; set; }

        public IdentityUser enseignant { get; set; }

        public IdentityUser etudiant { get; set; }

        public Evaluation evaluation { get; set; }

        public AutoEvaluation AutoEvaluation { get; set; }
        
        public List<DatePickerEvent> lstCours { get; set; }

        public bool Empty
        {
            get
            {
                return (cours==null);
            }
        }


    }
}
