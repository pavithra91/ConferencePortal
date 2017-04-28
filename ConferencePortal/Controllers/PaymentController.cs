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

            List<DeligateMapping> delMapList = new List<DeligateMapping>();

            var ID = (from config in en.Configurations
                             where config.ConventionID == 1
                             select new { BookingID = config.ConventionCode + config.BookingConfig.BookingID, AUTOID = config.BookingConfig.BookingID }).FirstOrDefault();

            string BookingID = ID.BookingID;

            Client cl = new Client();
            cl = cart.client;
            cl.ConventionID = 1;
            cl.BookingID = BookingID;
            en.Clients.Add(cl);
            en.SaveChanges();

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
                en.SaveChanges();
            }

            foreach (string items in form.AllKeys)
            {
                string value = form.GetValues(items).FirstOrDefault();

                DeligateMapping delMap = new DeligateMapping();
                string[] arr = items.Split('-');
                if(arr[0]=="AC")
                {
                    var id = (from saveDel in en.Deligates
                              where saveDel.BookingID == BookingID && saveDel.firstName == value select saveDel.AUTOID).FirstOrDefault();

                    delMap.Deligate = Convert.ToInt32(id);
                    delMap.RoomID = Convert.ToInt32(arr[1]);
                    delMapList.Add(delMap);
                }
            }
            return View();
        }
    }
}