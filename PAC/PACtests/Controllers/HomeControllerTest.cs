using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PAC.Controllers;
using PAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;


//using System.Web.Providers.Entities;

namespace PACtests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
       
        [TestMethod]
      
        public void Index()
        {

            //Arrange
 
                NavigationController Controller = new NavigationController();

            //Act
            ViewResult result = Controller.Index() as ViewResult;

            //Assert
            //Assert.AreEqual("index", result.ViewBag.Message);
        }



    
        /// ///lui il marche
        /// </summary>
        [TestMethod]
        public void Privacy()
        {
            //Arrange
            HomeController Controller = new HomeController();

            //Act
            ViewResult result = Controller.Privacy() as ViewResult;
            //Assert
            //Assert.AreEqual("Privacy Policy", result.ViewBag.Message);
            //Assert.IsNotNull(result);
        }
    }
}
