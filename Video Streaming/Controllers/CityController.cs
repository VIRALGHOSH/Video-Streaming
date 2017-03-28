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
    public class CityController : Controller
    {
        //
        // GET: /City/
        DataClasses1DataContext db = new DataClasses1DataContext();
        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Index()
        {
            return View(db.tbl_cities.ToList());
        }
        public ActionResult Print()
        {

            return View(db.tbl_cities.ToList());
        }
        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Create()
        {
            ViewBag.stateid = new SelectList(db.tbl_states.ToList(), "state_id", "state_name");
            ViewBag.countryid = new SelectList(db.tbl_countries.ToList(), "country_id", "country_name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(Model_city obj)
        {
            tbl_city tb = new tbl_city();
            tb.state_id = obj.state_id;
            tb.country_id = obj.country_id;
            tb.city_name = obj.city_name;
            db.tbl_cities.InsertOnSubmit(tb);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Edit(int id)
        {
            Model_city obj = db.tbl_cities.Where( x => x.city_id == id).Select(x => new Model_city()
            {
                city_id = x.city_id,
                state_id= x.state_id,
                country_id = x.country_id,
                city_name = x.city_name
            }).SingleOrDefault();
            ViewBag.stateid = new SelectList(db.tbl_states.ToList(), "state_id", "state_name",obj.state_id);
            ViewBag.countryid = new SelectList(db.tbl_countries.ToList(), "country_id", "country_name",obj.country_id);
            return View(obj);
        }
        [HttpPost]
           public ActionResult Edit(Model_city obj)
        {
            tbl_city tb = db.tbl_cities.Where(x => x.city_id == obj.city_id).Single<tbl_city>();
            tb.city_id = obj.city_id;
            tb.state_id = obj.state_id;
            tb.country_id = obj.country_id;
            tb.city_name = obj.city_name;
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Delete(Int16 id)
        {
            Model_city obj = db.tbl_cities.Where(x => x.city_id == id).Select(x => new Model_city()
            {
                city_id = x.city_id,
                state_id = x.state_id,
                country_id = x.country_id,
                city_name = x.city_name
            }).SingleOrDefault();
            return View(obj);
        }
        [HttpPost]
        public ActionResult Delete(Int32 id)
        {
            tbl_city tb = db.tbl_cities.Where(x => x.city_id == id).Single<tbl_city>();
            db.tbl_cities.DeleteOnSubmit(tb);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Loadstate(int id)
        {
            var data = from states in db.tbl_states
                       where states.country_id == id
                       select states;
            //  int id12 = data.Count();
            return Json(new SelectList(data, "state_id", "state_name"));
        }
    }
}
