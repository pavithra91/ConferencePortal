using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConferencePortal.Controllers.api
{
    public class ReservationapiController : Controller
    {
        conferencedbEntities en = new conferencedbEntities();
        // GET: Reservationapi
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string GetBookingStartPeriod(string ConventionID)
        {
            int Convention = Convert.ToInt32(1);

            var Configurations = from config in en.Configurations
                                 where config.ConventionID == Convention
                                 select config;


            string BookingStartDate = "";
            foreach (var items in Configurations)
            {
                DateTime newDate = Convert.ToDateTime(items.BookingPeriodStart);

                DateTime today = DateTime.Now;

                //if (newDate < today)
                //{
                //    newDate = today;
                //}

                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(newDate.Month);
                BookingStartDate = monthName + " " + newDate.Day + ", " + newDate.Year + " 11:13:00";

            }

            return BookingStartDate; //"March 31, 2017 11:13:00";
        }

        [HttpGet]
        public string GetBookingEndPeriod(string ConventionID)
        {
            int Convention = Convert.ToInt32(1);

            var Configurations = from config in en.Configurations
                                 where config.ConventionID == Convention
                                 select config;


            string BookingEndDate = "";
            foreach (var items in Configurations)
            {
                DateTime newDate = Convert.ToDateTime(items.BookingPeriodEnd);
                DateTime today = DateTime.Now;

                //if(newDate<today)
                //{
                //    newDate = today;
                //}

                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(newDate.Month);
                BookingEndDate = monthName + " " + newDate.Day + ", " + newDate.Year;

            }

            return BookingEndDate; //"March 31, 2017 11:13:00";
        }

        [HttpGet]
        public virtual JsonResult GetPickUpLocations(string ConventionID)
        {
            int Convention = Convert.ToInt32(1);

            string[] transport = en.Transports.Where(w => w.ShowInSearch == "Y" && w.Type == "A" && w.ConventionID == Convention).Select(w => w.StartLocation).ToArray();
            return Json(transport, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetDropOffLocations(string ConventionID)
        {
            int Convention = Convert.ToInt32(ConventionID);

            string[] transport = en.Transports.Where(w => w.ShowInSearch == "Y" && w.Type == "D" && w.ConventionID == Convention).Select(w => w.StartLocation).ToArray();
            return Json(transport, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetLocations(string ConventionID)
        {
            int Convention = Convert.ToInt32(ConventionID);

            string[] excursion = en.Excursions.Where(w => w.ConventionID == Convention).Select(w => w.Location).Distinct().ToArray();
            return Json(excursion, JsonRequestBehavior.AllowGet);
        }
    }
}