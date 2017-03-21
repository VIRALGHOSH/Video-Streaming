using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Video_Streaming.Dbml;
using Video_Streaming.Models;
using Video_Streaming.Filters;

namespace Video_Streaming.Controllers
{
    public class HomeController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        
    //  [CustomAuthorization_User(LoginPage="~/Login/signin")]
        public ActionResult Index()
        {
            List<Model_video> md = new List<Model_video>();
            var query = from v in db.tbl_videos
                        join a in db.tbl_views on v.video_id equals a.video_id into a_join
                        from a in a_join.DefaultIfEmpty()
                        group new { v, a } by new
                        {
                            v.video_name,
                            v.video_thumb,
                            v.video_date,
                            v.video_id
                        } into g
                        orderby
                          g.Count(p => p.a.video_id != null) descending
                        select new
                        {
                            TotalView = g.Count(p => p.a.video_id != null),
                            g.Key.video_id,
                            g.Key.video_name,
                            g.Key.video_thumb,
                            g.Key.video_date
                        }; 
            
            foreach (var data in query)
            {
                Model_video obj = new Model_video();
                obj.totalview = data.TotalView;
                obj.video_name = data.video_name;
                obj.video_id = data.video_id;
                obj.video_thumb = data.video_thumb;
                obj.login_id = (DateTime.Today - data.video_date).Days;
               // obj.video_date = data.video_date;
              //obj.video_date =  db.tbl_videos.Where(x => DbFunctions.DiffDays(x.video_date, DateTime.Now) == 0);
                md.Add(obj);
            }
            
            return View(md);
        }
      
     [CustomAuthorization_User(LoginPage = "~/Login/signin")]
     public ActionResult Sub_Category(int? id)
        {
            List<Model_video> md = new List<Model_video>();
            var query = from v in db.tbl_videos
                        join a in db.tbl_views on v.video_id equals a.video_id into a_join
                        from a in a_join.DefaultIfEmpty()
                        group new { v, a } by new
                        {
                            v.video_name,
                            v.video_thumb,
                            v.video_date,
                            v.video_id
                        } into g
                        orderby
                          g.Count(p => p.a.video_id != null) descending
                        select new
                        {
                            TotalView = g.Count(p => p.a.video_id != null),
                            g.Key.video_id,
                            g.Key.video_name,
                            g.Key.video_thumb,
                            g.Key.video_date
                        };
          if (id != null)
            {
               query = from v in db.tbl_videos
                        join a in db.tbl_views on v.video_id equals a.video_id into a_join
                        from a in a_join.DefaultIfEmpty()
                       where
                        v.sub_cat_id == id
                        group new { v, a } by new
                        {
                            v.video_name,
                            v.video_thumb,
                            v.video_date,
                            v.video_id
                        } into g
                        orderby
                          g.Count(p => p.a.video_id != null) descending
                        select new
                        {
                            TotalView = g.Count(p => p.a.video_id != null),
                            g.Key.video_id,
                            g.Key.video_name,
                            g.Key.video_thumb,
                            g.Key.video_date
                        };
            }
            else
            {
                query = from v in db.tbl_videos
                        join a in db.tbl_views on v.video_id equals a.video_id into a_join
                        from a in a_join.DefaultIfEmpty()
                        group new { v, a } by new
                        {
                            v.video_name,
                            v.video_thumb,
                            v.video_date,
                            v.video_id
                        } into g
                        orderby
                          g.Count(p => p.a.video_id != null) descending
                        select new
                        {
                            TotalView = g.Count(p => p.a.video_id != null),
                            g.Key.video_id,
                            g.Key.video_name,
                            g.Key.video_thumb,
                            g.Key.video_date
                        };
            }
            foreach (var data in query)
            {
                Model_video obj = new Model_video();
                obj.totalview = data.TotalView;
                obj.video_name = data.video_name;
                obj.video_id = data.video_id;
                obj.video_thumb = data.video_thumb;
                obj.login_id = (DateTime.Today - data.video_date).Days;
                // obj.video_date = data.video_date;
                //obj.video_date =  db.tbl_videos.Where(x => DbFunctions.DiffDays(x.video_date, DateTime.Now) == 0);
                md.Add(obj);
            }
            return View(md);
            
        }
     [CustomAuthorization_User(LoginPage = "~/Login/signin")]
     public ActionResult Category(int? id)
        {
            List<Model_video> md = new List<Model_video>();
            var query = from v in db.tbl_videos
                        join a in db.tbl_views on v.video_id equals a.video_id into a_join
                        from a in a_join.DefaultIfEmpty()
                        group new { v, a } by new
                        {
                            v.video_name,
                            v.video_thumb,
                            v.video_date,
                            v.video_id
                        } into g
                        orderby
                          g.Count(p => p.a.video_id != null) descending
                        select new
                        {
                            TotalView = g.Count(p => p.a.video_id != null),
                            g.Key.video_id,
                            g.Key.video_name,
                            g.Key.video_thumb,
                            g.Key.video_date
                        };
            if (id != null)
            {
                query = from v in db.tbl_videos
                        join a in db.tbl_views on v.video_id equals a.video_id into a_join
                        from a in a_join.DefaultIfEmpty()
                        where
                         v.cat_id == id
                        group new { v, a } by new
                        {
                            v.video_name,
                            v.video_thumb,
                            v.video_date,
                            v.video_id
                        } into g
                        orderby
                          g.Count(p => p.a.video_id != null) descending
                        select new
                        {
                            TotalView = g.Count(p => p.a.video_id != null),
                            g.Key.video_id,
                            g.Key.video_name,
                            g.Key.video_thumb,
                            g.Key.video_date
                        };
            }
            else
            {
                query = from v in db.tbl_videos
                        join a in db.tbl_views on v.video_id equals a.video_id into a_join
                        from a in a_join.DefaultIfEmpty()
                        group new { v, a } by new
                        {
                            v.video_name,
                            v.video_thumb,
                            v.video_date,
                            v.video_id
                        } into g
                        orderby
                          g.Count(p => p.a.video_id != null) descending
                        select new
                        {
                            TotalView = g.Count(p => p.a.video_id != null),
                            g.Key.video_id,
                            g.Key.video_name,
                            g.Key.video_thumb,
                            g.Key.video_date
                        };
            }
            foreach (var data in query)
            {
                Model_video obj = new Model_video();
                obj.totalview = data.TotalView;
                obj.video_name = data.video_name;
                obj.video_id = data.video_id;
                obj.video_thumb = data.video_thumb;
                obj.login_id = (DateTime.Today - data.video_date).Days;
                // obj.video_date = data.video_date;
                //obj.video_date =  db.tbl_videos.Where(x => DbFunctions.DiffDays(x.video_date, DateTime.Now) == 0);
                md.Add(obj);
            }
            return View(md);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult menu()
        {
            List<tbl_category> lscategory = db.tbl_categories.ToList();
            string str = "<li class='active'><span class='submenu-button'></span><a href='/Home'><i class='fi ion-ios-home'></i>Home</a> ";

            foreach (var item in lscategory)
            {
                List<tbl_sub_category> lssubcategory = db.tbl_sub_categories.Where(x => x.cat_id == item.cat_id).ToList();
                if (lssubcategory.Count > 0)
                {
                    str = str + "<li class='has-sub'><span class='submenu-button'></span><a href='/home/category/" + item.cat_id + "'><i class='fi ion-ios-film-outline'></i>" + item.cat_name + "</a>";
                    str = str + "		   	  <ul>";
                    foreach (var item1 in lssubcategory)
                    {
                        str = str + "  	<li><a href='/home/sub_category/" + item1.sub_cat_id + "'>" + item1.sub_cat_name + "</a></li>";
                    }
                    str = str + "  	  </ul>";
                    str = str + "  	</li>";
                }
                else
                {
                    str = str + "<li><a href='/home/category/" + item.cat_id + "'><i class='fi ion-ios-film-outline'></i>" + item.cat_name + "</a></li>";
                }

            }
            str = str + "<li class='active'><span class='submenu-button'></span><a href='/feedback/form'><i class='fi ion-person'></i>Contact Us</a>";
            return Json(str);
        }

    }
}
