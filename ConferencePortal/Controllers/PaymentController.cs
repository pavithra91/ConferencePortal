using ConferencePortal;
using ConferencePortal.App_Code;
using ConferencePortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConferencePortal.Controllers
{
    public class PaymentController : Controller
    {
        conferencedbEntities en = new conferencedbEntities();
        // GET: Payment
        public ActionResult Index(FormCollection form)
        {
            IEnumerable<Deligate> Deligates = TempData["Deligates"] as IEnumerable<Deligate>;
            ShoppingCart cart = TempData["ShoppingCart"] as ShoppingCart;
            double TotalPrice = 0.0;

            List<DeligateMapping> delMapList = new List<DeligateMapping>();

            var ID = (from config in en.Configurations
                             where config.ConventionID == 1
                             select new { BookingID = config.ConventionCode + config.BookingConfig.BookingID, AUTOID = config.BookingConfig.BookingID, PaymentOption = config.PaymentOption }).FirstOrDefault();

            string BookingID = ID.BookingID;

            Client cl = new Client();
            cl = cart.client;
            cl.ConventionID = 1;
            cl.BookingID = BookingID;
            en.Clients.Add(cl);
            //en.SaveChanges();

            BookingConfig bookingconfig = en.BookingConfigs.Where(s => s.ConferenceID == 1).FirstOrDefault<BookingConfig>();

            bookingconfig.BookingID = ID.AUTOID + 1;
            en.Entry(bookingconfig).State = System.Data.Entity.EntityState.Modified;
            en.SaveChanges();            

            foreach (Deligate del in Deligates)
            {
                Deligate d = new Deligate();
                d = del;
                d.BookingID = BookingID;
                en.Deligates.Add(d);
                //en.SaveChanges();
            }

            foreach (string items in form.AllKeys)
            {
                string value = form.GetValues(items).FirstOrDefault();

                DeligateMapping delMap = new DeligateMapping();
                string[] arr = items.Split('-');
                if(arr[0]=="AC")
                {
                    var id = (from saveDel in en.Deligates
                              where saveDel.BookingID == BookingID && saveDel.firstName == value
                              select saveDel.DeligateID).FirstOrDefault();

                    RoomReservation roomReservation = new RoomReservation();
                    roomReservation.DeligateID = Convert.ToInt32(id);

                    foreach(var rmCart in cart.Rooms)
                    {
                        int RoomId = Convert.ToInt32(arr[1]);
                        if (rmCart.room.RoomID== RoomId)
                        {
                            TotalPrice += (rmCart.Price/rmCart.NoofRooms);
                            roomReservation.CheckInDate = rmCart.CheckInDate;
                            roomReservation.CheckOutDate = rmCart.CheckOutDate;
                            
                            roomReservation.RoomID = RoomId;
                            en.RoomReservations.Add(roomReservation);
                            //en.SaveChanges();
                        }
                    }
                }
                if(arr[0] == "TR")
                {
                    var id = (from saveDel in en.Deligates
                              where saveDel.BookingID == BookingID && saveDel.firstName == value
                              select saveDel.DeligateID).FirstOrDefault();

                    TransportReservation TRReservation = new TransportReservation();
                    TRReservation.DeligateID = Convert.ToInt32(id);

                    foreach (var trCart in cart.Transport)
                    {
                        int TRID = Convert.ToInt32(arr[1]);
                        if (trCart.TR.TransportID == TRID)
                        {
                            TotalPrice += trCart.Price;
                            
                            TRReservation.PickUpDate = Convert.ToDateTime(trCart.PickUpDate);
                            //TRReservation.PickUpTime = Convert.ToDateTime(trCart.PickUpTime);
                            TRReservation.NoOfVehicles = trCart.NoOfVehicles;
                            TRReservation.Price = trCart.Price;
                            TRReservation.TransportType = trCart.TransportType;
                            TRReservation.RateID = trCart.TR.RateID;
                            en.TransportReservations.Add(TRReservation);
                            //en.SaveChanges();
                        }
                    }
                }
                if (arr[0] == "EX")
                {
                    var id = (from saveDel in en.Deligates
                              where saveDel.BookingID == BookingID && saveDel.firstName == value
                              select saveDel.DeligateID).FirstOrDefault();

                    ExcursionReservation ExReservation = new ExcursionReservation();
                    ExReservation.DeligateID = Convert.ToInt32(id);

                    foreach (var exCart in cart.Excursion)
                    {
                        int EXID = Convert.ToInt32(arr[1]);
                        if (exCart.Excursion.ExcursionsID == EXID)
                        {
                            TotalPrice += exCart.Price;
                            ExReservation.ExcursionDate = exCart.ExcursionDate;
                            ExReservation.NoOfAdults = exCart.NoOfAdults;
                            ExReservation.Price = exCart.Price;
                            en.ExcursionReservations.Add(ExReservation);
                            //en.SaveChanges();
                        }
                    }
                }

            }

            en.SaveChanges();

            string PaymentOption = form.GetValues("PaymentOption").FirstOrDefault();

            if(PaymentOption == "Pay Later")
            {
                TempData["Option"] = "Pay Later";
                return RedirectToAction("PaymentComplete", "Payment");
            }
            else
            {
                
            }

            return View();
        }

        public ActionResult PaymentComplete()
        {

            return View();
        }


    }
}