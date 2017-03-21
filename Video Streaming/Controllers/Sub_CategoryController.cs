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
    public class Sub_CategoryController : Controller
    {
        //
        // GET: /Sub_Category/
        DataClasses1DataContext db = new DataClasses1DataContext();

        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Index()
        {
            return View(db.tbl_sub_categories.ToList());
        }

        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Create()
        {
            ViewBag.categoryid = new SelectList(db.tbl_categories.ToList(), "cat_id", "cat_name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Model_sub_category obj)
        {
            tbl_sub_category tb = new tbl_sub_category();
            tb.cat_id = obj.cat_id;
            tb.sub_cat_name = obj.sub_cat_name;
            tb.sub_cat_des = obj.sub_cat_des;
            db.tbl_sub_categories.InsertOnSubmit(tb);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Edit(int id)
        {
            Model_sub_category obj = db.tbl_sub_categories.Where(x => x.sub_cat_id == id).Select(x => new Model_sub_category()
            {   
                sub_cat_id = x.sub_cat_id,
                cat_id = x.cat_id,
                sub_cat_name = x.sub_cat_name,
                sub_cat_des = x.sub_cat_des
            }).SingleOrDefault();
            ViewBag.categoryid = new SelectList(db.tbl_categories.ToList(), "cat_id", "cat_name",obj.cat_id);
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit(Model_sub_category obj)
        {
            tbl_sub_category tb = db.tbl_sub_categories.Where(x => x.sub_cat_id == obj.sub_cat_id).Single<tbl_sub_category>();
           
            tb.sub_cat_id = obj.sub_cat_id;
            tb.cat_id = obj.cat_id;
            tb.sub_cat_name = obj.sub_cat_name;
            tb.sub_cat_des = obj.sub_cat_des;
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Delete(Int16 id)
        {
            Model_sub_category obj = db.tbl_sub_categories.Where(x => x.sub_cat_id == id).Select(x => new Model_sub_category()
            {
                sub_cat_id = x.sub_cat_id,
                cat_id = x.cat_id,
                sub_cat_name = x.sub_cat_name,
                sub_cat_des = x.sub_cat_des
            }).SingleOrDefault();
            return View(obj);
        }

        [HttpPost]
        public ActionResult Delete(Int32 id)
        {
            tbl_sub_category tb = db.tbl_sub_categories.Where(x => x.sub_cat_id == id).Single<tbl_sub_category>();
            db.tbl_sub_categories.DeleteOnSubmit(tb);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}
