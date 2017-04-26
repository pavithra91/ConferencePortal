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
            IEnumerable<Registration> Deligates = TempData["Deligates"] as IEnumerable<Registration>;
            ShoppingCart cart = TempData["ShoppingCart"] as ShoppingCart;

            List<DeligateMapping> delMapList = new List<DeligateMapping>();

            string BookingID = (from config in en.Configurations
                                                  where config.ConventionID == 1
                                                  select config.ConventionCode + config.BookingConfig.BookingID).ToString();
            
            Client cl = new Client();
            cl = cart.client;
            cl.ConventionID = 1;
            cl.BookingID = BookingID;
            en.Clients.Add(cl);
            en.SaveChanges();

            foreach(Registration del in Deligates)
            {

            }

            foreach (string items in form.AllKeys)
            {
                string value = form.GetValues(items).FirstOrDefault();

                DeligateMapping delMap = new DeligateMapping();
                string[] arr = items.Split('-');
                if(arr[0]=="AC")
                {
                    var del = Deligates.Where(w => w.firstName == value);
                    delMap.Deligate = 1;
                    delMap.RoomID = Convert.ToInt32(arr[1]);
                    delMapList.Add(delMap);
                }
            }
            return View();
        }
    }
}