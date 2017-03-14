using ConferencePortal.App_Code;
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
            ViewBag.NoOfDeligates = 5;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterClient(Client cl, int noOfDeligate)
        {
            ShoppingCart cart = new ShoppingCart();
            cart.ClientId = cl.ClientID;
            cart.NoofDelegates = noOfDeligate;

            TempData["ShoppingCart"] = cart;

            return RedirectToAction("Index", "Reservation", new { ConventionID = 1, HotelId = 0 });
        }
    }
}