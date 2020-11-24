using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAC.Models
{
    public class Question
    {
        public int id { get; set; }
        public string question { get; set; }

        public int pointsTotal { get; set; }
        public int position { get; set; }
    }
}
