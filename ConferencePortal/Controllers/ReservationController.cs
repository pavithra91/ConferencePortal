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
        public ActionResult Index(int Hotel)
        {
            int ConventionID = 1;

            var Hotels = from st in en.Hotels
                         where st.ConferenceID == ConventionID
                         select st;

            var result = from st in en.Rooms
                           where st.ConferenceID == ConventionID && st.HotelID == Hotel
                           select st;

            ViewBag.Hotels = Hotels.ToList();
            ViewBag.Rooms = result.ToList();

            return View();
        }

        public ActionResult Test()
        {
            return Json(Url.Action("Index", "Account"));
        }
    }
}