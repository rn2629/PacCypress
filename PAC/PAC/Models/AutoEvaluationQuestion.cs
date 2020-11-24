using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAC.Models
{
    public class AutoEvaluationQuestion
    {
        public int questionId { get; set; }
        public int evaluationId { get; set; }
        public int? resultat { get; set; }
    }
}
