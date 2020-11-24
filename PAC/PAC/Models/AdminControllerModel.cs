using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PAC.Models
{
    public class AdminControllerModel
    {
        public List<IdentityUser> Enseignants { get; set; }
        public List<IdentityUser> ProfDesoutiens { get; set; }

    }
}
