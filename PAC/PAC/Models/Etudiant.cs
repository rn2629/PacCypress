using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PAC.Models
{
    public class Etudiant
    {
        public string Id { get; set; }
        public int NoDA { get; set; }

        public bool Jumeler  { get; set; }


    }
}
