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
        // GET: Payment
        public ActionResult Index(FormCollection form)
        {
            IEnumerable<Registration> Deligates = TempData["Deligates"] as IEnumerable<Registration>;
            ShoppingCart cart = TempData["ShoppingCart"] as ShoppingCart;

            List<DeligateMapping> delMapList = new List<DeligateMapping>();


            foreach (string items in form.AllKeys)
            {
                string value = form.GetValues(items).FirstOrDefault();

                DeligateMapping delMap = new DeligateMapping();
                string[] arr = items.Split('-');
                if(arr[0]=="AC")
                {
                    delMap.Deligate = 1;
                    delMap.RoomID = Convert.ToInt32(arr[1]);
                    delMapList.Add(delMap);
                }
            }
            return View();
        }
    }
}