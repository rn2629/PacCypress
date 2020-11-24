using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PAC.Models
{
    public class Disponibilite
    {
        public int Id { get; set; }
        public int priority{ get; set; }

        [DataType(DataType.Date)]
        public DateTime startTime { get; set; }
        [DataType(DataType.Date)]
        public DateTime endTime { get; set; }


    }
}
