using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAC.Models
{
    public class DatePickerSeeder
    {
        public static void Seed(DatePickerContext context)
        {
            if (context.tblSeanceCours.Any())
            {
                return;   // DB has been seeded
            }
        }
    }
}
