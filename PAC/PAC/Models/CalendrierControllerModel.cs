using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PAC.Models
{
    public class CalendrierControllerModel
    {
        public DatePickerEvent Cours { get; set; }
        public IdentityUser Prof { get; set; }
        public List<IdentityUser> Etudiants { get; set; }
    }
}
