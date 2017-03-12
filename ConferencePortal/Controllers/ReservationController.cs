using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConferencePortal.Controllers
{
    public class ReservationController : Controller
    {
        conferencedbEntities en = new conferencedbEntities();
        // GET: Reservation
        public ActionResult Index()
        {
            int ConventionID = 1;

            var result = from st in en.Rooms
                           where st.ConferenceID == ConventionID
                           select st;

            ViewBag.Rooms = result.ToList();

            return View();
        }

        public ActionResult Test()
        {
            return Json(Url.Action("Index", "Account"));
        }
    }
}