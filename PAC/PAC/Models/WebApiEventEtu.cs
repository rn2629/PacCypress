using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAC.Models
{
    public class WebApiEventEtu
    {
        public int id { get; set; }
        public string idUser { get; set; }
        public int priority { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }

        public static explicit operator WebApiEventEtu(DatePickerEventEtu ev)
        {
            
                return new WebApiEventEtu
                {
                    id = ev.id,
                    idUser = ev.etudiantId,
                    priority = ev.priority,
                    start_date = ev.startTime.ToString("yyyy-MM-dd HH:mm"),
                    end_date = ev.endTime.ToString("yyyy-MM-dd HH:mm")
                };
           
         

        }

        public static explicit operator DatePickerEventEtu(WebApiEventEtu ev)
        {
            return new DatePickerEventEtu
                { 
                    id = ev.id,
                    etudiantId = "",
                    priority = ev.priority,
                    startTime = DateTime.Parse(ev.start_date,
                    System.Globalization.CultureInfo.InvariantCulture),
                    endTime = DateTime.Parse(ev.end_date,
                    System.Globalization.CultureInfo.InvariantCulture)
                };

        }
    }
}
