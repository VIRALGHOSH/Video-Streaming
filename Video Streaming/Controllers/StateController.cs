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
    public class StateController : Controller
    {
        //
        // GET: /State/
        DataClasses1DataContext db = new DataClasses1DataContext();
        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Index()
        {
            return View(db.tbl_states.ToList());
        }
        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Create()
        {
            ViewBag.countryid = new SelectList(db.tbl_countries.ToList(), "country_id", "country_name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(Model_State obj)
        {
            tbl_state tb = new tbl_state();
            tb.country_id = obj.country_id;
            tb.state_name = obj.state_name;
            db.tbl_states.InsertOnSubmit(tb);
            db.SubmitChanges();

            return RedirectToAction("Index");
        }
        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Edit(int id)
        {
            Model_State obj = db.tbl_states.Where( x => x.state_id == id).Select(x => new Model_State()
            {

                state_id= x.state_id,
                country_id= x.country_id,
                state_name= x.state_name
            }).SingleOrDefault();
            ViewBag.countryid = new SelectList(db.tbl_countries.ToList(), "country_id", "country_name",obj.country_id);
            return View(obj);
        }
        [HttpPost]
        public ActionResult Edit(Model_State obj)
        {
           tbl_state tb = db.tbl_states.Where(x => x.state_id == obj.state_id).Single<tbl_state>();
           tb.state_id =   obj.state_id;
           tb.country_id = obj.country_id;
           tb.state_name = obj.state_name;
           db.SubmitChanges();
          
           return RedirectToAction("Index");
        }
        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Delete(Int32 id)
        {
            Model_State obj = db.tbl_states.Where(x => x.state_id == id).Select(x => new Model_State()
            {
                state_id = x.state_id,
                country_id = x.country_id,
                state_name = x.state_name
            }).SingleOrDefault();

            return View(obj);
        }
        [HttpPost]
        public ActionResult Delete(Int16 id)
        {
            tbl_state tb = db.tbl_states.Where(x => x.state_id == id).Single<tbl_state>();
            db.tbl_states.DeleteOnSubmit(tb);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}
