﻿using ConferencePortal.App_Code;
using ConferencePortal.Models;
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
        conferencedbEntities _context = new conferencedbEntities();
        // GET: Reservationapi
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string GetBookingStartPeriod(string ConventionID)
        {
            int Convention = Convert.ToInt32(1);

            var Configurations = from config in _context.Configurations
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

            var Configurations = from config in _context.Configurations
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

            string[] transport = _context.Transports.Where(w => w.ShowInSearch == "Y" && w.Type == "A" && w.ConventionID == Convention).Select(w => w.StartLocation).ToArray();
            return Json(transport, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetDropOffLocations(string ConventionID)
        {
            int Convention = Convert.ToInt32(ConventionID);

            string[] transport = _context.Transports.Where(w => w.ShowInSearch == "Y" && w.Type == "D" && w.ConventionID == Convention).Select(w => w.StartLocation).ToArray();
            return Json(transport, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetLocations(string ConventionID)
        {
            int Convention = Convert.ToInt32(ConventionID);

            string[] excursion = _context.Excursions.Where(w => w.ConventionID == Convention).Select(w => w.Location).Distinct().ToArray();
            return Json(excursion, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetHotelDescription(string DescriptionID)
        {
            try
            {
                int ID = Convert.ToInt32(DescriptionID);

                return Json(
                    _context.HotelDescriptions.Where(w => w.DescID == ID).Select(x => new
                    {
                        shortdesc = x.ShortDescription,
                        longdesc = x.LognDescription
                    }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public virtual int SaveHotelDescription(ConfigModel objModel)
        {
            try
            {
                if(objModel.DescID==0)
                {
                    HotelDescription objHotelDesc = new HotelDescription();
                    objHotelDesc.HotelID = objModel.HotelID;
                    objHotelDesc.ShortDescription = objModel.ShortDescription;
                    objHotelDesc.LognDescription = objHotelDesc.LognDescription;

                    _context.HotelDescriptions.Add(objHotelDesc);
                    _context.SaveChanges();
                    return 1;
                }
                else
                {
                    HotelDescription objHotelDesc = _context.HotelDescriptions.Where(w=>w.DescID == objModel.DescID).FirstOrDefault();
                    objHotelDesc.ShortDescription = objModel.ShortDescription;
                    objHotelDesc.LognDescription = objHotelDesc.LognDescription;

                    _context.Entry(objHotelDesc).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                    return 1;
                }                
            }
            catch
            {
                return 0;
            }
        }

        [HttpPost]
        public ActionResult AddtoCart(ShoppingCart objModel)
        {
            ShoppingCart cart = TempData["ShoppingCart"] as ShoppingCart;

            if (objModel.ItemType == "AC")
            {
                TempDate TempDates = TempData["TempDate"] as TempDate;

                DateTime start = TempDates.StartDate;
                DateTime end = TempDates.EndDate;

                int roomCount = Convert.ToInt32(objModel.Count);
                double RoomPrice = Convert.ToDouble(objModel.Rate);
                Room room = _context.Rooms.Find(Convert.ToInt32(objModel.ItemID));
                RoomsInCart rmCart = new RoomsInCart();
                rmCart.room = room;
                rmCart.Price = RoomPrice * roomCount;
                rmCart.NoofRooms = roomCount;
                rmCart.CheckInDate = start;
                rmCart.CheckOutDate = end;

                //cart.TotalPrice += RoomPrice * roomCount;
                cart.Rooms.Add(rmCart);
            }
            else if (objModel.ItemType == "TR")
            {
                TempDate TempDates = TempData["TRTempDate"] as TempDate;

                int VehicleCount = Convert.ToInt32(objModel.Count);
                double Price = Convert.ToDouble(objModel.Rate);
                TransportRate TR = _context.TransportRates.Find(Convert.ToInt32(objModel.ItemID));
                TransportInCart TRCart = new TransportInCart();
                TRCart.TR = TR;
                TRCart.Price = Price;// * VehicleCount;
                TRCart.NoOfVehicles = VehicleCount;
                if (TR.Transport.Type == "A")
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

                cart.TotalPrice += Price;// * VehicleCount;
                cart.Transport.Add(TRCart);
            }

            else if (objModel.ItemType == "EX")
            {
                TempDate TempDates = TempData["EXTempDate"] as TempDate;

                int AdultCount = Convert.ToInt32(objModel.Count);
                double Price = Convert.ToDouble(objModel.Rate);
                Excursion EX = _context.Excursions.Find(Convert.ToInt32(objModel.ItemID));
                ExcursionsInCart Excursions = new ExcursionsInCart();
                Excursions.ExcursionDate = TempDates.StartDate;
                Excursions.Excursion = EX;
                Excursions.Price = Price; // * AdultCount;
                Excursions.NoOfAdults = AdultCount;

                cart.TotalPrice += Price; // * AdultCount;
                cart.Excursion.Add(Excursions);
            }

            return RedirectToAction("ViewCart", "Reservation", new { ConventionID = cart.ConventionID });
        }
    }
}