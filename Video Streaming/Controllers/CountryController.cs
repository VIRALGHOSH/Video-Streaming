using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Video_Streaming.Dbml;
using Video_Streaming.Filters;
using Video_Streaming.Models;

namespace Video_Streaming.Controllers
{
    public class CountryController : Controller
    {
        //
        // GET: /Country/

        DataClasses1DataContext db = new DataClasses1DataContext();
        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Index()
        {
            return View(db.tbl_countries.ToList());
        }

        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Model_country obj)
        {
            tbl_country tb = new tbl_country();
            tb.country_name = obj.country_name;
            db.tbl_countries.InsertOnSubmit(tb);
            db.SubmitChanges();

            return RedirectToAction("Index");
        }

        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Edit(int id)
        {
            Model_country obj = db.tbl_countries.Where(x => x.country_id == id).Select(x => new Model_country()
            {
                country_id = x.country_id,
                country_name = x.country_name
            }).SingleOrDefault();
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit(Model_country obj)
        {
            tbl_country tb = db.tbl_countries.Where(x => x.country_id == obj.country_id).Single<tbl_country>();
            tb.country_name = obj.country_name;
            tb.country_id = obj.country_id;
            db.SubmitChanges();

            return RedirectToAction("Index");
        }

        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Delete(Int16 id)
        {
            Model_country obj = db.tbl_countries.Where(x => x.country_id == id).Select(x => new Model_country()
            {
                country_id = x.country_id,
                country_name = x.country_name
            }).SingleOrDefault();
            return View(obj);
        }

        [HttpPost]
        public ActionResult Delete(Int32 id)
        {
            tbl_country tb = db.tbl_countries.Where(x => x.country_id == id).Single<tbl_country>();
            db.tbl_countries.DeleteOnSubmit(tb);
            db.SubmitChanges();

            return RedirectToAction("Index");
        }
    }
}
