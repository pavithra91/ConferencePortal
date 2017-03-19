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

            var Configurations = from config in en.Configurations
                              where config.ConventionID == Convention
                              select config;

            if (HotelId != 0)
            {
                var roomResult = from st in en.Rooms
                                 where st.ConventionID == Convention && st.HotelID == HotelId && st.Allotment.AvailableRooms > 0
                                 select st;

                ViewBag.Rooms = roomResult.ToList();
            }

            ViewBag.Configurations = Configurations.ToList();
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

        [HttpPost]
        public ActionResult SearchTransport()
        {

            return RedirectToAction("Index", "Reservation", new { ConventionID = 1, Transport = 1 });
        }


        public ActionResult ViewCart()
        {
            ShoppingCart cart = TempData["ShoppingCart"] as ShoppingCart;

            if (cart != null)
            {
                ViewBag.Rooms = cart.Rooms;
                ViewBag.Transport = cart.Transport;

                List<Client> cl = new List<Client> { cart.client };
                ViewBag.Client = cl;

                TempData["ShoppingCart"] = cart;
            }
            return View();
        }

        public ActionResult RemoveFromCart(string RoomId)
        {
            ShoppingCart cart = TempData["ShoppingCart"] as ShoppingCart;

            List<Room> rm = cart.Rooms;
            foreach(var items in rm)
            {
                if(items.RoomID == Convert.ToInt32(RoomId))
                {
                    rm.Remove(items);
                    break;
                }
            }

            cart.Rooms = rm;

            ViewBag.Rooms = cart.Rooms;
            ViewBag.Transport = cart.Transport;

            TempData["ShoppingCart"] = cart;

            return RedirectToAction("ViewCart", "Reservation");
        }

        public ActionResult Test()
        {
            return Json(Url.Action("Index", "Account"));
        }
    }
}