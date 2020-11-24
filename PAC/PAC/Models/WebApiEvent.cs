using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAC.Models
{
    public class WebApiEvent
    {
        public int id { get; set; }
        public string idUser { get; set; }
        public string text { get; set; }
        public string descrip { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }

        public static explicit operator WebApiEvent(DatePickerEvent ev)
        {
            if (ev.local != null)
            {
                return new WebApiEvent
                {
                    id = ev.id,
                    idUser = ev.enseignantId,
                    text = ev.nomCours,
                    descrip = ev.local,
                    start_date = ev.startTime.ToString("yyyy-MM-dd HH:mm"),
                    end_date = ev.endTime.ToString("yyyy-MM-dd HH:mm")
                };
            }
            else
            {
                return new WebApiEvent
                {
                    id = ev.id,
                    idUser = ev.enseignantId,
                    text = ev.nomCours,
                    descrip = "",
                    start_date = ev.startTime.ToString("yyyy-MM-dd HH:mm"),
                    end_date = ev.endTime.ToString("yyyy-MM-dd HH:mm")
                };
            }
            
        }

        public static explicit operator DatePickerEvent(WebApiEvent ev)
        {

            if (ev.descrip != null)
            {
                return new DatePickerEvent
                {
                    id = ev.id,
                    enseignantId = "",
                    nomCours = ev.text,
                    local = ev.descrip,
                    startTime = DateTime.Parse(ev.start_date,
                    System.Globalization.CultureInfo.InvariantCulture),
                    endTime = DateTime.Parse(ev.end_date,
                    System.Globalization.CultureInfo.InvariantCulture)
                };
            }
            else
            {
                return new DatePickerEvent
                {
                    id = ev.id,
                    enseignantId = "",
                    nomCours = ev.text,
                    local = "",
                    startTime = DateTime.Parse(ev.start_date,
                    System.Globalization.CultureInfo.InvariantCulture),
                    endTime = DateTime.Parse(ev.end_date,
                    System.Globalization.CultureInfo.InvariantCulture)
                };
            }
            
        }
    }
}
