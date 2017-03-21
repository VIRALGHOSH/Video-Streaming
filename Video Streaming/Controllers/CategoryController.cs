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
    public class CategoryController : Controller
    {
        //
        // GET: /Category/
        DataClasses1DataContext db = new DataClasses1DataContext();

        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Index()
        {
            return View(db.tbl_categories.ToList());
        }

        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Model_category obj)
        {
            tbl_category tb = new tbl_category();
            tb.cat_name = obj.cat_name;
            tb.cat_des = obj.cat_des;
            db.tbl_categories.InsertOnSubmit(tb);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Edit(int id)
        {
            Model_category obj = db.tbl_categories.Where(x => x.cat_id == id).Select(x => new Model_category()
            {
                cat_id= x.cat_id,
                cat_name = x.cat_name,
                cat_des = x.cat_des
            }).SingleOrDefault();
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit(Model_category obj)
        {
            tbl_category tb = db.tbl_categories.Where(x => x.cat_id == obj.cat_id).Single<tbl_category>();
           
            tb.cat_id = obj.cat_id;
            tb.cat_name = obj.cat_name;
            tb.cat_des = obj.cat_des;
            db.SubmitChanges();
            return RedirectToAction("Index");
         
        }

        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Delete(Int16 id)
        {
             Model_category obj = db.tbl_categories.Where(x => x.cat_id == id).Select(x => new Model_category()
             {
                 cat_id = x.cat_id,
                 cat_name = x.cat_name,
                 cat_des = x.cat_des
             }).SingleOrDefault();
             return View(obj);
        }

        [HttpPost]
        public ActionResult Delete(Int32 id)
        {
            tbl_category tb = db.tbl_categories.Where(x => x.cat_id == id).Single<tbl_category>();
            db.tbl_categories.DeleteOnSubmit(tb);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}
