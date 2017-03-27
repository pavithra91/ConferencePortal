﻿using ConferencePortal.App_Code;
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
        public ActionResult Index(string ConventionID, int HotelId)
        {
            ShoppingCart cart = TempData["ShoppingCart"] as ShoppingCart;            

            int Convention = Convert.ToInt32(ConventionID);

            var hotelResult = from st in en.Hotels
                              where st.ConventionID == Convention
                              select st;

            var Configurations = from config in en.Configurations
                              where config.ConventionID == Convention
                              select config;

            if (HotelId != 0)
            {
                TempDate TempDates = TempData["TempDate"] as TempDate;

                if (TempDates!=null)
                {
                    DateTime start = TempDates.StartDate;
                    DateTime end = TempDates.EndDate;

                    var rooms =
    (from roomAllotment in en.RoomAllotments
     from rms in en.Rooms
     where roomAllotment.Room.ConventionID == Convention  && (roomAllotment.Date >= start && roomAllotment.Date <= end) 
     select new { roomAllotment.Date, roomAllotment.AvailableRooms, roomAllotment.RoomRate.Allotment, roomAllotment.Room.RoomName, roomAllotment.Room }).Distinct().ToList();


                    List<Room> rm = new List<Room>();
                    foreach(var items in rooms)
                    {
                        if((items.Allotment+items.AvailableRooms)>0)
                        {
                            rm.Add(items.Room);
                        }
                        else
                        {
                            rm.Remove(items.Room);
                        }
                    }

                    List<Room> Result = rm.Distinct().ToList();
                    ViewBag.Rooms = Result.ToList();
                }
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
            return Json("March 31, 2017 11:13:00");
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