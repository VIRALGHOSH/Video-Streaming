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
    public class ComplainController : Controller
    {
        //
        // GET: /Complain/
        DataClasses1DataContext db = new DataClasses1DataContext();

        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Index()
        {
            Session["feedbackunread"] = db.tbl_feedbacks.Where(x => x.f_status == false).Count();
            Session["complainunread"] = db.tbl_complains.Where(x => x.com_status == false).Count();
            return View(db.tbl_complains.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Form()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Form(Model_complain obj)
        {
            tbl_complain tb = new tbl_complain();
            tb.login_id = SessionData.UserId;
            tb.com_sub = obj.com_sub;
            tb.com_des = obj.com_des;
            tb.com_status = false;
            tb.com_date = DateTime.Now;
            db.tbl_complains.InsertOnSubmit(tb);
            db.SubmitChanges();

            return RedirectToAction("Index");
        }
        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Details(Int16 id)
         {
             Model_complain obj = db.tbl_complains.Where(x => x.com_id == id).Select(x => new Model_complain()
            {
                com_id = x.com_id,
                com_sub = x.com_sub,
                com_des = x.com_des,
                com_date = x.com_date,
                com_status = x.com_status
            }).SingleOrDefault();
             var str = "";
             if (obj.com_status == true)
             {

                 str = "Approved";
             }
             else
             {
                 str = "Pending";
             }
             ViewBag.comstatus = str;
             return View(obj);
         }
         [HttpPost]
             public ActionResult Details(Int32 id)
         {
            tbl_complain tbl = db.tbl_complains.Where( x => x.com_id == id).Single<tbl_complain>();
             tbl.com_status= true;
             db.SubmitChanges();
             Session["complainunread"] = db.tbl_complains.Where(x => x.com_status == false).Count();
             return RedirectToAction("Index");
         }

         [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
         public ActionResult Delete(Int16 id)
        {
            Model_complain obj = db.tbl_complains.Where(x => x.com_id == id).Select(x => new Model_complain()
           {
               login_id = x.login_id,
               com_sub = x.com_sub,
               com_des = x.com_des
           }).SingleOrDefault();


            return View(obj);
        }
        [HttpPost]
        public ActionResult Delete(Int32 id)
        {
            tbl_complain tb = db.tbl_complains.Where(x => x.com_id == id).Single<tbl_complain>();
            db.tbl_complains.DeleteOnSubmit(tb);
            db.SubmitChanges();
            Session["complainunread"] = db.tbl_complains.Where(x => x.com_status == false).Count();
            return RedirectToAction("Index");
        }
    }
}


