using ConferencePortal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConferencePortal.Controllers
{
    public class ConfigController : Controller
    {
        conferencedbEntities _context = new conferencedbEntities();
        // GET: Config
        public ActionResult Index()
        {
            Session["ConventionID"] = "1";
            return View();
        }

        public ActionResult ConventionConfigurations()
        {
            Session["ConventionID"] = "1";
            if (Session["ConventionID"] == null)
            {
                return null;
            }

            int ConventionId = Convert.ToInt32(Session["ConventionID"].ToString());

            IEnumerable<Configuration> config = _context.Configurations.Where(w => w.ConventionID == ConventionId);

            ViewBag.Configurations = config;
            return View();
        }

        public ActionResult HotelManager()
        {
            Session["ConventionID"] = "1";
            if (Session["ConventionID"] == null)
            {
                return null;
            }

            ConfigModel objModel = new ConfigModel();
            objModel.hotelList = _context.ConventionHotels.Where(w=>w.ConventionID == 1).Select(w=>w.Hotel).ToList();
            return View(objModel);
        }

        public ActionResult ManageHotel(int id)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("Index", "Account");
            //}

            if (id > 0)
            {
                Hotel _hotel = _context.Hotels.Where(m => m.HotelID == id).FirstOrDefault();
                if (_hotel != null)
                {
                    ConfigModel objModel = new ConfigModel();
                    objModel._hotel = _hotel;
                    return View(objModel);
                }
            }
            else
            {
                ConfigModel objModel = new ConfigModel();
                objModel._hotel = new Hotel();

                return View(objModel);
            }
            return null;
        }

        public ActionResult ManageHotelDescription(string HotelID)
        {
            ConfigModel objModel = new ConfigModel();
            objModel._hotelDescriptionList = new List<HotelDescription>();
            objModel._hotelDescriptionList = _context.HotelDescriptions.Where(w => w.HotelID == 1).ToList();
            return View(objModel);
        }

        public ActionResult SaveDescription(FormCollection objModel)
        {

            return null;
        }

        [HttpPost]
        public ActionResult SaveHotel(ConfigModel objModel)
        {
            return null;
        }

        public ActionResult AllotmentManager()
        {
            return View();
        }

        public ActionResult RateManager()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveSettings(FormCollection form, HttpPostedFileBase Logo, IEnumerable<HttpPostedFileBase> file)
        {
            if (Logo != null)
            {
                //var fileName = Path.GetFileName(file.FileName);
                //var path = Path.Combine(Server.MapPath("~/App_Data/Images"), fileName);
                //file.SaveAs(path);
            }

            if(file != null)
            {
                int Count = 1;
                foreach(var files in file)
                {
                    if (files != null)
                    {
                        var fileName = "img" + Count + ".jpg";
                        var path = Path.Combine(Server.MapPath("~/img/slider"), fileName);
                        files.SaveAs(path);
                        Count++;
                    }
                }
            }
            return RedirectToAction("ConventionConfigurations", "Config", new { ConventionID = "1"});
        }
    }
}