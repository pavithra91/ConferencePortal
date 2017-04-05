using ConferencePortal.App_Code;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConferencePortal.Controllers
{
    public class ReservationController : Controller
    {
        conferencedbEntities en = new conferencedbEntities();
        // GET: Reservation
        public ActionResult Index(string ConventionID)
        {

            ShoppingCart cart = TempData["ShoppingCart"] as ShoppingCart;            

            int Convention = Convert.ToInt32(ConventionID);

            var hotelResult = from st in en.Hotels
                              where st.ConventionID == Convention
                              select st;

            IEnumerable<Transport> transport = en.Transports.Where(w => w.ShowInSearch=="Y");

            List<Configuration> Configurations = (from config in en.Configurations
                              where config.ConventionID == Convention
                              select config).ToList();
           

            ViewBag.Configurations = Configurations;
            ViewBag.Hotels = hotelResult.ToList();

            //ViewBag.Arrival = transport;//new SelectList(transport, "StartLocation", "StartLocation");
            //ViewBag.Depature = new SelectList(transport, "DropOffLocation", "DropOffLocation");
            ViewBag.Transport = transport;

            IEnumerable<Room> testRooms = TempData["HotelRooms"] as IEnumerable<Room>;
            IEnumerable<RoomAllotment> allotmentListRooms = TempData["Allotments"] as IEnumerable<RoomAllotment>;

            if (testRooms != null)
            {
                ViewBag.Rooms = testRooms;
                ViewBag.Allotments = allotmentListRooms;
            }

            TempData["ShoppingCart"] = cart;

            return View();
        }

        public ActionResult HotelView(string ConventionID, int HotelId)
        {
            int Convention = Convert.ToInt32(ConventionID);

            if (HotelId != 0)
            {
                TempDate TempDates = TempData["TempDate"] as TempDate;

                if (TempDates != null)
                {
                    DateTime start = TempDates.StartDate;
                    DateTime end = TempDates.EndDate;

                    IEnumerable<RoomAllotment> allotmentListRooms = en.RoomAllotments
                        .Where(w => w.Date >= start && w.Date <= end && w.Room.ConventionID == Convention);

                    IEnumerable<Room> testRooms = allotmentListRooms.Select(w => w.Room);
                    IEnumerable<Room> roomsToBeRemoved = allotmentListRooms.Where(w => (w.AvailableRooms.HasValue ? w.AvailableRooms.Value : 0) <= 0).Select(w => w.Room);
                    testRooms = testRooms.Except(roomsToBeRemoved);


                    //var rooms =
                    //            (from roomAllotment in en.RoomAllotments
                    //            where roomAllotment.Room.ConventionID == Convention  && (roomAllotment.Date >= start && roomAllotment.Date <= end) 
                    //            select new { RoomCount = roomAllotment.AvailableRooms + roomAllotment.RoomRate.Allotment, roomAllotment.Date, roomAllotment.AvailableRooms,
                    //                roomAllotment.RoomRate.Allotment, roomAllotment.Room.RoomName, roomAllotment.Room }).Distinct().ToList();


                    //List<Room> RoomList = new List<Room>();
                    //List<Room> RemoveList = new List<Room>();

                    //foreach(var items in rooms)
                    //{
                    //    if((items.Allotment+items.AvailableRooms)>0)
                    //    {
                    //        RoomList.Add(items.Room);
                    //    }
                    //    else
                    //    {
                    //        RemoveList.Add(items.Room);
                    //    }
                    //}

                    //foreach(var items in RemoveList)
                    //{
                    //    RoomList.Remove(items);
                    //}

                    //List<Room> Result = RoomList.Distinct().ToList();

                    //List<TempRoom> tmroom = new List<TempRoom>();
                    //foreach(var items in Result)
                    //{
                    //    TempRoom tm = new TempRoom();
                    //    tm.RoomID = items.RoomID;

                    //    var rooms2 =
                    //            (from roomAllotment in en.RoomAllotments
                    //             where roomAllotment.RoomID == items.RoomID
                    //             select new { RoomCount = roomAllotment.AvailableRooms + roomAllotment.RoomRate.Allotment }).Distinct();



                    //    tmroom.Add(tm);
                    //}

                    TempData["HotelRooms"] = testRooms;
                    TempData["Allotments"] = allotmentListRooms;
                }
            }

            return RedirectToAction("Index", "Reservation", new { ConventionID = 1});
        }

        public ActionResult AddtoCart(string RoomId)
        {
            string roomCount = Request.Form["roomCount"];

            ShoppingCart cart = TempData["ShoppingCart"] as ShoppingCart;
    
            Room room = en.Rooms.Find(Convert.ToInt32(RoomId));
            cart.Rooms.Add(room);

            return RedirectToAction("Index", "Reservation", new { ConventionID = 1, HotelId = 0 });
        }

        [HttpPost]
        public ActionResult SearchHotel(FormCollection fomr)
        {
            string HotelList = Request.Form["HotelList"];
            string start = Request.Form["start"];
            string end = Request.Form["end"];

            DateTime startdate = Convert.ToDateTime(start);
            DateTime enddate = Convert.ToDateTime(end);

            TempDate TempDates = new TempDate();
            TempDates.StartDate = startdate;
            TempDates.EndDate = enddate;

            TempData["TempDate"] = TempDates;

            return RedirectToAction("HotelView", "Reservation", new { ConventionID = 1, HotelId = Convert.ToInt32(HotelList) });
        }

        [HttpPost]
        public ActionResult SearchTransport(FormCollection fomr)
        {
            string Pickup = Request.Form["Pickup"];
            string DropOff = Request.Form["DropOff"];

            return RedirectToAction("Index", "Reservation", new { ConventionID = 1});
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

        public JsonResult Test(string type)
        {
            List<string> team = new List<string>();
            IEnumerable<Transport> transport = en.Transports.Where(w => w.ShowInSearch == "Y" && w.Type == "A");

            foreach (var items in transport)
            {
                team.Add(items.StartLocation);
            }

            return Json(team);
        }

        public string GetBookingStartPeriod(string ConventionID)
        {
            int Convention = Convert.ToInt32(ConventionID);

            var Configurations = from config in en.Configurations
                                 where config.ConventionID == Convention
                                 select config;


            string BookingStartDate = "";
            foreach(var items in Configurations)
            {
               DateTime newDate = Convert.ToDateTime(items.BookingPeriodStart);
               
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(newDate.Month);
                    BookingStartDate = monthName + " " + newDate.Day + ", " + newDate.Year + " 11:13:00";

            }

            return BookingStartDate; //"March 31, 2017 11:13:00";
        }

        public string GetBookingEndPeriod(string ConventionID)
        {
            int Convention = Convert.ToInt32(ConventionID);

            var Configurations = from config in en.Configurations
                                 where config.ConventionID == Convention
                                 select config;


            string BookingEndDate = "";
            foreach (var items in Configurations)
            {
                DateTime newDate = Convert.ToDateTime(items.BookingPeriodEnd);

                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(newDate.Month);
                BookingEndDate = monthName + " " + newDate.Day + ", " + newDate.Year + " 11:13:00";

            }

            return BookingEndDate; //"March 31, 2017 11:13:00";
        }

    }
}