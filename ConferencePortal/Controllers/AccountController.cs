using ConferencePortal.App_Code;
using ConferencePortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ConferencePortal.Controllers
{
    public class AccountController : Controller
    {
        conferencedbEntities en = new conferencedbEntities();
        // GET: Account
        public ActionResult Index()
        {
            ViewBag.ConventionID = "1";
            
            return View();
        }

        public ActionResult DeligateRegistration(string ConventionID)
        {
            ShoppingCart cart = TempData["ShoppingCart"] as ShoppingCart;

            int Convention = Convert.ToInt32(ConventionID);

            var Configurations = from config in en.Configurations
                                 where config.ConventionID == Convention
                                 select config;

            ViewBag.Configurations = Configurations.ToList();

            ViewBag.NoOfDeligates = 2;

            if (cart.client.Deligate == true)
            {
                List<Client> cl = new List<Client> { cart.client };
                ViewBag.Client = cl;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterClient(Client cl, int noOfDeligate, string[] Delegate)
        {
            ShoppingCart cart = new ShoppingCart();
            cart.ClientId = cl.ClientID;
            cart.NoofDelegates = noOfDeligate;

            if(Delegate != null)
            {
                cl.Deligate = true;
            }

            cart.client = cl;

            TempData["ShoppingCart"] = cart;

            return RedirectToAction("Index", "Reservation", new { ConventionID = 1});
        }

        [HttpPost]
        public ActionResult RegisterDelegates(IEnumerable<Registration> reg)
        {

            return RedirectToAction("Index", "Reservation", new { ConventionID = 1, HotelId = 0 });
        }

        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authenticate(string Email, string Passwrod)
        {
            FormsAuthentication.SignOut();
            LoginDataModel dataIn = new LoginDataModel();

            try
            {
                if (TryUpdateModel(dataIn))
                {
                    string groups = "";

                    bool isCookiePersistent = dataIn.rememberMe;
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, dataIn.username, DateTime.Now, DateTime.Now.AddMinutes(45), isCookiePersistent, groups);
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                    if (true == isCookiePersistent)
                    {
                        authCookie.Expires = authTicket.Expiration;
                        Response.Cookies.Add(authCookie);
                    }

                    else
                    {
                        if (Email.Equals(null) || Passwrod.Equals(null))
                        {
                            return RedirectToAction("Login", "Account");
                        }

                        var User = from config in en.Clients
                                             where config.email == Email
                                             select config;

                        //var User = entity.ExternalUserTBLs.SqlQuery("SELECT * FROM dbo.ExternalUserTBL WHERE dbo.ExternalUserTBL.UserName = '" + Email + "';").ToList();


                        bool status = UserAuthentication(User, Passwrod);

                        if (status)
                        {
                            return RedirectToAction("Index", "Account");
                        }
                        else
                        {
                            return RedirectToAction("UserProfile", "Account");
                            //return RedirectToAction("Login", "Account");
                        }
                    }

                }
            }
            catch(Exception ex)
            {

            }
            return RedirectToAction("Index", "Reservation", new { ConventionID = 1, HotelId = 0 });
        }

        public bool UserAuthentication(IEnumerable<dynamic> list, string Password)
        {
            foreach (var result in list)
            {
                if (result == null)
                {
                    return false;
                }
                else if (result.IsUserVerified == false)
                {
                    return false;
                }
                else if (result.Password != Password)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
    }
}