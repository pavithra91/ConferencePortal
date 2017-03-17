using ConferencePortal.App_Code;
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
        public ActionResult Index(string ConventionID, int HotelId)
        {
            ShoppingCart cart = TempData["ShoppingCart"] as ShoppingCart;

            int Convention = Convert.ToInt32(ConventionID);

            var hotelResult = from st in en.Hotels
                              where st.ConferenceID == Convention
                              select st;

            if (HotelId != 0)
            {
                var roomResult = from st in en.Rooms
                                 where st.ConventionID == Convention && st.HotelID == HotelId
                                 select st;

                ViewBag.Rooms = roomResult.ToList();
            }

            ViewBag.Hotels = hotelResult.ToList();
            TempData["ShoppingCart"] = cart;

            return View();
        }

        public ActionResult AddtoCart(string RoomId)
        {
            ShoppingCart cart = TempData["ShoppingCart"] as ShoppingCart;
    
            Room room = en.Rooms.Find(Convert.ToInt32(RoomId));
            cart.Rooms.Add(room);

            return RedirectToAction("Index", "Reservation", new { ConventionID = 1, HotelId = 0 });
        }

        [HttpPost]
        public ActionResult SearchHotel(string HotelList)
        {
            string hot = HotelList;

            return RedirectToAction("Index", "Reservation", new { ConventionID = 1, HotelId = Convert.ToInt32(HotelList) });
        }

        public ActionResult ViewCart()
        {
            ShoppingCart cart = TempData["ShoppingCart"] as ShoppingCart;

            ViewBag.Rooms = cart.Rooms;
            ViewBag.Transport = cart.Transport;

            return View();
        }

        public ActionResult Test()
        {
            return Json(Url.Action("Index", "Account"));
        }
    }
}