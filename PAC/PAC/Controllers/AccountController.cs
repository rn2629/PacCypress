using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PAC.Models;


namespace PAC.Controllers
{
    public class AccountController : Controller
    {
       public ActionResult Login()
        {
            return LocalRedirect("~/Identity/Account/Login");
        }
    }
}
