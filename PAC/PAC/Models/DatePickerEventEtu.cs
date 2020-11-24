using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAC.Models
{
    public class DatePickerEventEtu
    {
        public int id { get; set; }
        public string etudiantId { get; set; }
        public int priority { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public DatePickerEventEtu(int id, string etudiantId, int priority, DateTime startTime, DateTime endTime)
        {
            this.id = id;
            this.etudiantId = etudiantId;
            this.priority = priority;
            this.startTime = startTime;
            this.endTime = endTime;
        }
        public DatePickerEventEtu()
        {

        }
    }
}
