using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Video_Streaming.Dbml;
using Video_Streaming.Models;
using Video_Streaming.Filters;

namespace Video_Streaming.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        DataClasses1DataContext db = new DataClasses1DataContext();

        [CustomAuthorization_Admin(LoginPage="/admin/signin")]
        public ActionResult Index()
        {
            ViewBag.complain = db.tbl_complains.Count();
            Session["complainunread"] = db.tbl_complains.Where(x => x.com_status == false).Count();
            ViewBag.feedback = db.tbl_feedbacks.Count();
            ViewBag.usercount = db.tbl_registrations.Count();
            
            ViewBag.todayviewcount = db.tbl_views.Where(x=>x.view_date.Day == DateTime.Now.Day).Count();
            ViewBag.videos = db.tbl_videos.Count();
            Session["feedbackunread"] = db.tbl_feedbacks.Where(x => x.f_status == false).Count();
            ViewBag.views = db.tbl_views.Count();
            ViewBag.videostranscoded = ViewBag.videos * 3;
            return View();
        }
        public ActionResult Signin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signin(Model_login obj)
        {
            if (obj.email == "viralghosh" && obj.password == "viral.com")
            {
                SessionData.AdminId = 1;
                return RedirectToAction("Index");
            }
            ViewBag.error = "Please check your username n password";
            return RedirectToAction("Signin");
        }

    }
}
