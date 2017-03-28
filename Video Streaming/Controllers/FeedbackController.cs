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
    public class FeedbackController : Controller
    {
        //
        // GET: /Feedback/
        DataClasses1DataContext db = new DataClasses1DataContext();

        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Index()
        {
            Session["feedbackunread"] = db.tbl_feedbacks.Where(x => x.f_status == false).Count();
            Session["complainunread"] = db.tbl_complains.Where(x => x.com_status == false).Count();
            return View(db.tbl_feedbacks.ToList());
        }
        public ActionResult Print()
        {

            return View(db.tbl_feedbacks.ToList());
        }
        public ActionResult Form()
        {
            return View();
        }
         [HttpPost]
        public ActionResult Form(Model_feedback obj)
        {
            tbl_feedback tb = new tbl_feedback();
            tb.login_id = SessionData.UserId; ;
            tb.f_name = obj.f_name;
            tb.f_email = obj.f_email;
            tb.f_des = obj.f_des;
            tb.f_status = false;
            tb.f_date = DateTime.Now;
            db.tbl_feedbacks.InsertOnSubmit(tb);
            db.SubmitChanges();
            return RedirectToAction("Index","Home");
        }
         [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
         public ActionResult Details(Int16 id)
         {
             Model_feedback obj = db.tbl_feedbacks.Where(x => x.f_id == id).Select(x => new Model_feedback()
            {
                f_id = x.f_id,
                f_name = x.f_name,
                f_email = x.f_email,
                f_des = x.f_des,
                f_date = x.f_date,
                f_status = x.f_status
            }).SingleOrDefault();
             var str = "";
             if (obj.f_status == true)
             {

                 str = "Approved";
             }
             else
             {
                 str = "Pending";
             }
             ViewBag.fstatus = str;
             return View(obj);
         }
         [HttpPost]
             public ActionResult Details(Int32 id)
         {
            tbl_feedback tbl = db.tbl_feedbacks.Where( x => x.f_id == id).Single<tbl_feedback>();
             tbl.f_status= true;
             db.SubmitChanges();
             return RedirectToAction("Index");
         }


         [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
         public ActionResult Delete(Int16 id)
         {
             Model_feedback obj = db.tbl_feedbacks.Where(x => x.f_id == id).Select(x => new Model_feedback()
             {
                 login_id = x.login_id,
                 f_des = x.f_des
             }).SingleOrDefault();
             Session["feedbackunread"] = db.tbl_feedbacks.Where(x => x.f_status == false).Count();
             return View(obj);
         }
         [HttpPost]
         public ActionResult Delete(Int32 id)
         {
             tbl_feedback tb = db.tbl_feedbacks.Where(x => x.f_id == id).Single<tbl_feedback>();
             db.tbl_feedbacks.DeleteOnSubmit(tb);
             db.SubmitChanges();
             Session["feedbackunread"] = db.tbl_feedbacks.Where(x => x.f_status == false).Count();
             return RedirectToAction("Index");
         }
    }
}
