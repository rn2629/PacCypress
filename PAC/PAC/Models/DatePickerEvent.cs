using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAC.Models
{
    public class DatePickerEvent
    {
        public int id { get; set; }
        public string enseignantId { get; set; }
        public string nomCours { get; set; }
        public string local { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }


        public DatePickerEvent()
        {

        }
        public DatePickerEvent(int id, string enseignantId, string nomCours, string local, DateTime startTime, DateTime endTime)
        {
            this.id = id;
            this.enseignantId = enseignantId;
            this.nomCours = nomCours;
            this.local = local;
            this.startTime = startTime;
            this.endTime = endTime;
        }
    }
}
