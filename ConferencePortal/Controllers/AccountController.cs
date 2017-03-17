using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConferencePortal.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            ViewBag.ConventionID = "001";
            
            return View();
        }

        public ActionResult DeligateRegistration()
        {
            ViewBag.NoOfDeligates = 10;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterClient(Client cl, string noOfDeligate)
        {
            
            return null;
        }
    }
}