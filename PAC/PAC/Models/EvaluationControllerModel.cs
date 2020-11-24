using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PAC.Models
{
    public class EvaluationControllerModel
    {
        public IEnumerable<EvaluationQuestion> LstEvaluationQuestion { get; set; }
        public IEnumerable<Question> LstQuestion { get; set; }

        public Evaluation Evaluation { get; set;}

        public AutoEvaluation AutoEvaluation { get; set; }
        public IEnumerable<AutoEvaluationQuestion> LstAutoEvaluationQuestion { get; set; }

        public IdentityUser Enseignant;

        public IdentityUser Etudiant;
    }
}
