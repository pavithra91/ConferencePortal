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
        conferencedbEntities en = new conferencedbEntities();
        // GET: Config
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ConventionConfigurations(string ConventionID)
        {
            int ConventionId = Convert.ToInt32(ConventionID);

            IEnumerable<Configuration> config = en.Configurations.Where(w => w.ConventionID == ConventionId);

            ViewBag.Configurations = config;
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