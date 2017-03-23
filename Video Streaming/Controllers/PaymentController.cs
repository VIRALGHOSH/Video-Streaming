using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Video_Streaming.Dbml;
using Video_Streaming.Models;
using NReco.VideoConverter;
using Video_Streaming.Filters;
namespace Video_Streaming.Controllers
{
    public class PaymentController : Controller
    {
        //
        // GET: /Payment/
        DataClasses1DataContext db = new DataClasses1DataContext();

        public ActionResult Index()
        {
            return View(db.tbl_subscriptions.ToList());
        }
        public ActionResult Active()
        {
            return View(db.tbl_subscriptions.Where(x => x.sub_end_date > DateTime.Now).ToList());
        }
        //[CustomAuthorization_User(LoginPage = "~/Login/signin")]
       
        public ActionResult Paypal()
        {
            if (db.tbl_subscriptions.Where(x => x.sub_end_date > DateTime.Now && SessionData.ustatus == "Free").Count() > 0)
            {
                tbl_subscription tb = new tbl_subscription();
                tb.login_id = SessionData.UserId;
                tb.sub_pay_date = DateTime.Now;
                tb.sub_end_date = DateTime.Today.AddMonths(1);
                tb.sub_amt = 5;
                db.tbl_subscriptions.InsertOnSubmit(tb);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
          else if (db.tbl_subscriptions.Where(x => x.sub_end_date > DateTime.Now && SessionData.ustatus == "Paid").Count() > 0)
            {
                tbl_subscription tb = new tbl_subscription();
                tb.login_id = SessionData.UserId;
                tb.sub_pay_date = DateTime.Now;
                tb.sub_end_date = DateTime.Today.AddMonths(1);
                tb.sub_amt = 5;
                db.tbl_subscriptions.InsertOnSubmit(tb);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
