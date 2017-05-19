using ConferencePortal.App_Code;
using ConferencePortal.Models;
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
            //TempDate TempDates = TempData["TempDate"] as TempDate;

            ShoppingCart cart = TempData["ShoppingCart"] as ShoppingCart;            

            int Convention = Convert.ToInt32(ConventionID);

            var hotelResult = from st in en.Hotels
                              where st.ConventionID == Convention
                              select st;

            //IEnumerable<Transport> transport = en.Transports.Where(w => w.ShowInSearch=="Y");

            List<Configuration> Configurations = (from config in en.Configurations
                              where config.ConventionID == Convention
                              select config).ToList();
           

            ViewBag.Configurations = Configurations;
            ViewBag.Hotels = hotelResult.ToList();
            
            //ViewBag.Transport = transport;

            IEnumerable<Room> testRooms = TempData["HotelRooms"] as IEnumerable<Room>;
            IEnumerable<RoomAllotment> allotmentListRooms = TempData["Allotments"] as IEnumerable<RoomAllotment>;
            IEnumerable<TransportRate> TrRates = TempData["Transport"] as IEnumerable<TransportRate>;
            IEnumerable<Excursion> Excursion = TempData["Excursion"] as IEnumerable<Excursion>;

            if (testRooms != null)
            {
                ViewBag.Rooms = testRooms;
                ViewBag.Allotments = allotmentListRooms;

                //ViewBag.StartDate = TempDates.StartDate;
                //ViewBag.EndDate = TempDates.EndDate;
            }
            else if(TrRates != null)
            {
                ViewBag.Transport = TrRates;
            }
            else if(Excursion != null)
            {
                ViewBag.Excursion = Excursion;
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

        //public ActionResult TransportView(string ConventionID, int TransportID)
        //{

        //}

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
            string TRType = fomr["myRadios"].ToString();
            if (TRType != "3")
            {
                string StartLocation = fomr["PickUp"].ToString();
                string EndLocation = fomr["DropOff"].ToString();
                string _PickUpDate = "";
                string _DropOffDate = "";
                TempDate TempDates = new TempDate();

                if (TRType=="1")
                {
                    _PickUpDate = fomr["start"].ToString();
                    DateTime PickUpDate = Convert.ToDateTime(_PickUpDate);
                    TempDates.StartDate = PickUpDate;
                    TempDates.PickUpTime = fomr["PickUpTime"].ToString();
                }
                else if(TRType == "2")
                {
                    _DropOffDate = fomr["end"].ToString();
                    TempDates.DropOffTime = fomr["DropOffTime"].ToString();
                    DateTime DropOffDate = Convert.ToDateTime(_DropOffDate);
                    TempDates.StartDate = DropOffDate;
                }     

                IEnumerable<TransportRate> TrRates = en.TransportRates
        .Where(w => w.Transport.StartLocation == StartLocation && w.Transport.DropOffLocation == EndLocation && w.Transport.ConventionID == 1);

                TempData["Transport"] = TrRates;

                TempData["TRTempDate"] = TempDates;
            }
            else
            {

            }

            return RedirectToAction("Index", "Reservation", new { ConventionID = 1});
        }

        [HttpPost]
        public ActionResult SearchExcursion(FormCollection fomr)
        {
            string Location = Request.Form["Location"];
            string start = Request.Form["start"];

            DateTime startdate = Convert.ToDateTime(start);

            TempDate TempDates = new TempDate();
            TempDates.StartDate = startdate;

            TempData["EXTempDate"] = TempDates;

            IEnumerable<Excursion> Excursion = en.Excursions
                .Where(w => w.Location == Location);

            TempData["Excursion"] = Excursion;

            return RedirectToAction("Index", "Reservation", new { ConventionID = 1 });
        }

        public ActionResult ViewCart()
        {
            ShoppingCart cart = TempData["ShoppingCart"] as ShoppingCart;

            if (cart != null)
            {
                ViewBag.Rooms = cart.Rooms;
                ViewBag.Transport = cart.Transport;
                ViewBag.Excursion = cart.Excursion;

                List<Client> cl = new List<Client> { cart.client };
                ViewBag.Client = cl;

                ViewBag.ConventionID = cart.ConventionID;

                TempData["ShoppingCart"] = cart;
            }
            return View();
        }

        public ActionResult AddtoCart(string ItemType, string ItemID, string Rate, string Count)
        {
            ShoppingCart cart = TempData["ShoppingCart"] as ShoppingCart;

            if (ItemType == "AC")
            {
                TempDate TempDates = TempData["TempDate"] as TempDate;

                DateTime start = TempDates.StartDate;
                DateTime end = TempDates.EndDate;

                int roomCount = Convert.ToInt32(Count);
                double RoomPrice = Convert.ToDouble(Rate);
                Room room = en.Rooms.Find(Convert.ToInt32(ItemID));
                RoomsInCart rmCart = new RoomsInCart();
                rmCart.room = room;
                rmCart.Price = RoomPrice * roomCount;
                rmCart.NoofRooms = roomCount;
                rmCart.CheckInDate = start;
                rmCart.CheckOutDate = end;

                cart.TotalPrice += RoomPrice * roomCount;
                cart.Rooms.Add(rmCart);
            }
            else if (ItemType == "TR")
            {
                TempDate TempDates = TempData["TRTempDate"] as TempDate;

                int VehicleCount = Convert.ToInt32(Count);
                double Price = Convert.ToDouble(Rate);
                TransportRate TR = en.TransportRates.Find(Convert.ToInt32(ItemID));
                TransportInCart TRCart = new TransportInCart();
                TRCart.TR = TR;
                TRCart.Price = Price * VehicleCount;
                TRCart.NoOfVehicles = VehicleCount;
                if(TR.Transport.Type=="A")
                {
                    TRCart.TransportType = "A";
                    TRCart.PickUpDate = TempDates.StartDate.ToShortDateString();
                    TRCart.PickUpTime = TempDates.PickUpTime;
                }
                else
                {
                    TRCart.TransportType = "D";
                    TRCart.PickUpDate = TempDates.EndDate.ToShortDateString();
                    TRCart.PickUpTime = TempDates.DropOffTime;
                }

                cart.TotalPrice += Price * VehicleCount;
                cart.Transport.Add(TRCart);
            }

            else if (ItemType == "EX")
            {
                TempDate TempDates = TempData["EXTempDate"] as TempDate;

                int AdultCount = Convert.ToInt32(Count);
                double Price = Convert.ToDouble(Rate);
                Excursion EX = en.Excursions.Find(Convert.ToInt32(ItemID));
                ExcursionsInCart Excursions = new ExcursionsInCart();
                Excursions.ExcursionDate = TempDates.StartDate;
                Excursions.Excursion = EX;
                Excursions.Price = Price * AdultCount;
                Excursions.NoOfAdults = AdultCount;

                cart.TotalPrice += Price * AdultCount;
                cart.Excursion.Add(Excursions);
            }
            
            //return RedirectToAction("Index", "Reservation", new { ConventionID = 1 });
            return RedirectToAction("ViewCart", "Reservation", new { ConventionID = cart.ConventionID });
        }

        public ActionResult RemoveFromCart(string ItemType, string ItemID)
        {
            ShoppingCart cart = TempData["ShoppingCart"] as ShoppingCart;

            if (ItemType == "AC")
            {
                List<RoomsInCart> rm = cart.Rooms;
                foreach (var items in rm)
                {
                    if (items.room.RoomID == Convert.ToInt32(ItemID))
                    {
                        rm.Remove(items);
                        break;
                    }
                }

                cart.Rooms = rm;
            }
            else if(ItemType == "TR")
            {
                List<TransportInCart> tr = cart.Transport;
                foreach (var items in tr)
                {
                    if (items.TR.TransportID == Convert.ToInt32(ItemID))
                    {
                        tr.Remove(items);
                        break;
                    }
                }

                cart.Transport = tr;
            }

            else if (ItemType == "EX")
            {
                List<ExcursionsInCart> ex = cart.Excursion;
                foreach (var items in ex)
                {
                    if (items.Excursion.ExcursionsID == Convert.ToInt32(ItemID))
                    {
                        ex.Remove(items);
                        break;
                    }
                }

                cart.Excursion = ex;
            }

            ViewBag.Rooms = cart.Rooms;
            ViewBag.Transport = cart.Transport;
            ViewBag.Excursion = cart.Excursion;

            ViewBag.ConventionID = cart.ConventionID;

            TempData["ShoppingCart"] = cart;

            return RedirectToAction("ViewCart", "Reservation");
        }

        public ActionResult ServiceList(string ConventionID)
        {
            IEnumerable<Deligate> Deligates = TempData["Deligates"] as IEnumerable<Deligate>;
            ShoppingCart cart = TempData["ShoppingCart"] as ShoppingCart;

            if (cart != null)
            {
                ViewBag.Rooms = cart.Rooms;
                ViewBag.Transport = cart.Transport;
                ViewBag.Excursion = cart.Excursion;
                ViewBag.TotalPrice = cart.TotalPrice;

                List<Client> cl = new List<Client> { cart.client };
                ViewBag.Client = cl;

                int ConventionId = Convert.ToInt32(cart.ConventionID);

                Configuration ID = en.Configurations
                        .Where(w => w.ConventionID == ConventionId).FirstOrDefault();

                string[] paymentOptionsArr = ID.PaymentOption.Split('-');
                List<string> paymentOptions = new List<string>();
                DateTime today = DateTime.Now;

                if (today > ID.LastPaymentDate)
                {
                    paymentOptions.Add("Pay Now");
                }
                else
                {
                    if (paymentOptionsArr[0] == "1")
                    {
                        paymentOptions.Add("Pay Now");
                    }

                    if (paymentOptionsArr[1] == "1")
                    {
                        paymentOptions.Add("Pay Later");
                    }
                    if (paymentOptionsArr[2] == "1")
                    {
                        paymentOptions.Add("Partial Payment");
                    }
                }

                ViewBag.PaymentOptions = paymentOptions;
            }

            ViewBag.Deligates = Deligates;

            TempData["Deligates"] = Deligates;
            TempData["ShoppingCart"] = cart;       

            return View();
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
            int Convention = Convert.ToInt32(1);

            var Configurations = from config in en.Configurations
                                 where config.ConventionID == Convention
                                 select config;


            string BookingStartDate = "";
            foreach(var items in Configurations)
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