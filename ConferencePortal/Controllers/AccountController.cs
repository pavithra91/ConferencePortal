using ConferencePortal.App_Code;
using ConferencePortal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            try
            {
                IEnumerable<ConventionImage> imgs = en.ConventionImages.Where(w => w.Name == "Slider");

                ViewBag.Images = imgs;
                ViewBag.ConventionID = "1";
            }
            catch(Exception ex)
            {

            }
             
            return View();
        }

        public ActionResult DeligateRegistration(string command)
        {
            ShoppingCart cart = TempData["ShoppingCart"] as ShoppingCart;
            
            int ConventionId = Convert.ToInt32(cart.ConventionID);

            TempData["ShoppingCart"] = cart;

            if (command == "AddServices")
            {        
                return RedirectToAction("Index", "Reservation", new { ConventionID = ConventionId });
            }
            else
            {
                var Configurations = from config in en.Configurations
                                     where config.ConventionID == ConventionId
                                     select config;


                ViewBag.Configurations = Configurations.ToList();

                ViewBag.NoOfDeligates = cart.NoofDelegates;

                if (cart.client.Deligate == true)
                {
                    List<Client> cl = new List<Client> { cart.client };
                    ViewBag.Client = cl;
                }

                return View();
            }
        }

        public ActionResult UserProfile(string BookingId)
        {
            IEnumerable<RoomReservation> RoomList = en.RoomReservations
                        .Where(w => w.Deligate.BookingID == BookingId);

            IEnumerable<TransportReservation> Transportlist = en.TransportReservations
                        .Where(w => w.Deligate.BookingID == BookingId);

            IEnumerable<ExcursionReservation> ExcursionList = en.ExcursionReservations
                        .Where(w => w.Deligate.BookingID == BookingId);

            IEnumerable<Client> Client = en.Clients
                        .Where(w => w.BookingID == BookingId);

            ViewBag.Client = Client;
            ViewBag.RoomList = RoomList;
            ViewBag.Transportlist = Transportlist;
            ViewBag.ExcursionList = ExcursionList;

            return View();
        }

        public ActionResult Payment(string BookingID)
        {
            IEnumerable<Payment> pay = en.Payments.Where(w => w.BookingID == BookingID);
            IEnumerable<Client> cl = en.Clients.Where(w => w.BookingID == BookingID);

            ViewBag.PaymentHistory = pay;
            ViewBag.Client = cl;

            return View();
        }

        public ActionResult AccountActivation(string ID)
        {
            int ClientID = Convert.ToInt32(ID);
            var client = en.Clients.Where(w => w.ClientID == ClientID);

            if (client != null)
            {
                client.FirstOrDefault().IsUserVerified = true;

                en.Entry(client).State = System.Data.Entity.EntityState.Modified;
                en.SaveChanges();
            }

            return RedirectToAction("SignIn", "Account");
        }

        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterClient(Client cl, int noOfDeligate, string[] IsUserDelegate)
        {
            ShoppingCart cart = new ShoppingCart();
            cart.ClientId = cl.ClientID;
            cart.NoofDelegates = noOfDeligate;
            cart.ConventionID = "1";

            if (IsUserDelegate != null)
            {
                cl.Deligate = true;
            }

            cart.client = cl;

            TempData["ShoppingCart"] = cart;

            return RedirectToAction("Index", "Reservation", new { ConventionID = 1});
        }

        [HttpPost]
        public ActionResult RegisterDelegates(IEnumerable<Deligate> reg)
        {
            TempData["Deligates"] = reg;

            ShoppingCart cart = TempData["ShoppingCart"] as ShoppingCart;
            TempData["ShoppingCart"] = cart;

            return RedirectToAction("ServiceList", "Reservation", new { ConventionID = cart.ConventionID });
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

                        Client User = (from config in en.Clients
                                             where config.email == Email
                                             select config).FirstOrDefault();

                        bool status = UserAuthentication(User, Passwrod);

                        if (status)
                        {
                            return RedirectToAction("UserProfile", "Account", new { BookingId = User.BookingID });
                            //return RedirectToAction("Index", "Account");
                        }
                        else
                        {
                            
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


        #region Private Methods
        private bool UserAuthentication(Client client, string Password)
        {
            if (client == null)
            {
                return false;
            }
            else if (client.IsUserVerified == false)
            {
                return false;
            }
            else if (client.Password != Password)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }
}