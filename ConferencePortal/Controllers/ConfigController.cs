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

        public ActionResult ManageHotelDescription(string id)
        {
            int _hotelID = Convert.ToInt32(id);
            ConfigModel objModel = new ConfigModel();
            objModel._hotelDescriptionList = new List<HotelDescription>();
            objModel._hotelDescriptionList = _context.HotelDescriptions.Where(w => w.HotelID == _hotelID).ToList();
            return View(objModel);
        }

        public ActionResult RoomManager(int id)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("Index", "Account");
            //}

            List<Room> _rooms = _context.Rooms.Where(m => m.HotelID == id).ToList();
            if (_rooms != null)
            {
                ConfigModel objModel = new ConfigModel();
                objModel.roomList = _rooms;
                Hotel _hotel = _context.Hotels.Where(w => w.HotelID == id).FirstOrDefault();
                TempData["Hotel"] = _hotel;
                //objModel._hotel = _hotel;
                return View(objModel);
            }
            return null;
        }

        public ActionResult ManageRoom(int id)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("Index", "Account");
            //}

            Hotel _hotel = TempData["Hotel"] as Hotel;

            if (id > 0)
            {
                Room _room = _context.Rooms.Where(m => m.RoomID == id).FirstOrDefault();
                if (_room != null)
                {
                    ConfigModel objModel = new ConfigModel();
                    objModel._room = _room;
                    objModel._hotel = _hotel;
                    return View(objModel);
                }
            }
            else
            {
                ConfigModel objModel = new ConfigModel();
                objModel._room = new Room();
                objModel._hotel = _hotel;

                return View(objModel);
            }
            return null;
        }

        public ActionResult ManageRoomDescription(string id)
        {
            Hotel _hotel = TempData["Hotel"] as Hotel;
            int _roomID = Convert.ToInt32(id);
            ConfigModel objModel = new ConfigModel();
            objModel._hotel = _hotel;
            objModel._roomDescriptionList = new List<RoomDescription>();
            objModel._roomDescriptionList = _context.RoomDescriptions.Where(w => w.RoomID == _roomID).ToList();
            return View(objModel);
        }

        [HttpPost]
        public ActionResult SaveHotel(ConfigModel objModel)
        {
            if(objModel._hotel.HotelID==0)
            {
                Hotel _hotel = new Hotel();
                _hotel.HotelCode = objModel._hotel.HotelCode;
                _hotel.HotelName = objModel._hotel.HotelName;
                _hotel.Address = objModel._hotel.Address;
                _hotel.StarRaing = objModel._hotel.StarRaing;
                _hotel.IsActive = true;

                _context.Hotels.Add(_hotel);
                _context.SaveChanges();

                return RedirectToAction("HotelManager","Config");
            }
            else
            {
                Hotel _hotel = _context.Hotels.Where(w => w.HotelID == objModel._hotel.HotelID).FirstOrDefault();
                if(_hotel != null)
                {
                    _hotel.HotelName = objModel._hotel.HotelName;
                    _hotel.Address = objModel._hotel.Address;
                    _hotel.StarRaing = objModel._hotel.StarRaing;
                    _hotel.IsActive = objModel._hotel.IsActive;

                    _context.Entry(_hotel).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("HotelManager", "Config");
                }
            }
            return null;
        }

        public ActionResult TariffManager()
        {
            Session["ConventionID"] = "1";
            if (Session["ConventionID"] == null)
            {
                return null;
            }

            ConfigModel objModel = new ConfigModel();
            objModel.hotelList = _context.ConventionHotels.Where(w => w.ConventionID == 1).Select(w => w.Hotel).ToList();
            return View(objModel);
        }

        public ActionResult AllotmentManager()
        {
            return View();
        }

        public ActionResult RateManager(int id)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("Index", "Account");
            //}

            List<Room> _rooms = _context.Rooms.Where(m => m.HotelID == id).ToList();
            List<Room> list = _context.RoomRates.Where(w => w.ConventionID == 1).Select(w=>w.Room).Distinct().ToList();

            if (_rooms != null)
            {
                ConfigModel objModel = new ConfigModel();
                objModel.roomList = _rooms;
                Hotel _hotel = _context.Hotels.Where(w => w.HotelID == id).FirstOrDefault();
                TempData["Hotel"] = _hotel;
                objModel.RateAlreadyAssign = list;
                return View(objModel);
            }
            return null;
        }

        public ActionResult ManageRoomTariff(int Id, bool? ShowDeleted)
        {
            Session["ConventionID"] = "1";
            if (Session["ConventionID"] == null)
            {
                return null;
            }

            TariffModel objModel = new TariffModel();
            objModel._room = _context.Rooms.Where(w => w.RoomID == Id).FirstOrDefault();
            if (ShowDeleted == null || ShowDeleted == false)
            {
                objModel._roomRateList = _context.RoomRates.Where(w => w.RoomID == Id && w.Status == true).ToList();
                objModel._showDeleted = false;
            }
            else
            {
                objModel._showDeleted = true;
                objModel._roomRateList = _context.RoomRates.Where(w => w.RoomID == Id).ToList();
            }
            return View(objModel);
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