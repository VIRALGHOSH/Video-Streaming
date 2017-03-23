using Amazon.S3;
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
    public class VideoController : Controller
    {
        //
        // GET: /Video/
        DataClasses1DataContext db = new DataClasses1DataContext();
      
        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Index()
        {
            
            return View(db.tbl_videos.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.categoryid = new SelectList(db.tbl_categories.ToList(), "cat_id", "cat_name");
            ViewBag.subcategoryid = new SelectList(db.tbl_sub_categories.ToList(), "sub_cat_id", "sub_cat_name");   
            return View();
        }
        [HttpPost]
        public ActionResult Create(Model_video obj)
        {
            HttpPostedFileBase file = Request.Files["file1"];
            tbl_video tb = new tbl_video();
               if (file.FileName != "")
            {
                var filename = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/fileupload"), filename);
                file.SaveAs(path);
                S3Class s3obj = new S3Class();
                string str = s3obj.putObject("transcoder.thumbnail.video.streaming", path, file.FileName);
                tb.video_thumb = str;
            }
            tb.login_id = obj.login_id;
            tb.cat_id = obj.cat_id;
            tb.sub_cat_id = obj.sub_cat_id;
            tb.video_name = obj.video_name;
            tb.video_des = obj.video_des;
            tb.video_path = obj.video_path;
            tb.video_date = DateTime.Today.Date;
            db.tbl_videos.InsertOnSubmit(tb);

            db.SubmitChanges();
            return RedirectToAction("Index");
         }

        public ActionResult Edit(int id)
        {
            Model_video obj = db.tbl_videos.Where(x => x.video_id == id).Select(x => new Model_video()
            {
                video_id = x.video_id,
                login_id = x.login_id,
                cat_id = x.cat_id,
                sub_cat_id = x.sub_cat_id,
                video_name = x.video_name,
                video_des = x.video_des,
                video_path = x.video_path,
                video_thumb = x.video_thumb
            }).SingleOrDefault();
            ViewBag.categoryid = new SelectList(db.tbl_categories.ToList(), "cat_id", "cat_name",obj.cat_id);
            ViewBag.subcategoryid = new SelectList(db.tbl_sub_categories.ToList(), "sub_cat_id", "sub_cat_name",obj.sub_cat_id);   
            return View(obj);
        }
        [HttpPost]
        public ActionResult Edit(Model_video obj)
        {
            HttpPostedFileBase file = Request.Files["file1"];
            tbl_video tb = db.tbl_videos.Where(x => x.video_id == obj.video_id).Single<tbl_video>();
            if (file.FileName != "")
            {
                var filename = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/fileupload"), filename);
                file.SaveAs(path);
                S3Class s3obj = new S3Class();
                string str = s3obj.putObject("all.input.video.streaming", path, file.FileName.Replace(' ','_'));
                tb.video_path = str;

            }
            tb.cat_id = obj.cat_id;
            tb.sub_cat_id = obj.sub_cat_id;
            tb.video_name = obj.video_name;
            tb.video_des = obj.video_des;
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        [CustomAuthorization_User(LoginPage = "~/Login/signin")]
        public ActionResult upload()
        {
            ViewBag.categoryid = new SelectList(db.tbl_categories.ToList(), "cat_id", "cat_name");
            ViewBag.subcategoryid = new SelectList(db.tbl_sub_categories.ToList(), "sub_cat_id", "sub_cat_name");
            return View();
        }
        [HttpPost]
        public ActionResult upload(Model_video obj)
        {
            HttpPostedFileBase file = Request.Files["file1"];
            tbl_video tb = new tbl_video();
              if (file.FileName != "")
            {
                var filename = Path.GetFileName(file.FileName);
                string keyName = filename;
                int fileExtPos = keyName.LastIndexOf(".");
                if (fileExtPos >= 0)
                    keyName = keyName.Substring(0, fileExtPos);
                var path = Path.Combine(Server.MapPath("~/fileupload"), filename);
                file.SaveAs(path);
                string thumbnailJPEGpath = Server.MapPath("~/fileupload/" + keyName + ".jpg");
                FFMpegConverter ffmpeg = new FFMpegConverter();
                ffmpeg.GetVideoThumbnail(path, thumbnailJPEGpath, 4);
                var path2 = Path.Combine(Server.MapPath("~/fileupload"), keyName + ".jpg");
                S3Class s3obj = new S3Class();
                string str2 = s3obj.putObject("transcoder.input.video.streaming", path, file.FileName.Replace(' ', '_'));
                string str = s3obj.putObject("transcoder.thumbnail.video.streaming", path2, keyName + ".jpg");
                tb.video_thumb = str;
                tb.video_path = str2;
            }
            tb.login_id = obj.login_id;
            tb.cat_id = obj.cat_id;
            tb.sub_cat_id = obj.sub_cat_id;
            tb.video_name = obj.video_name;
            tb.video_des = obj.video_des;
            tb.video_date = DateTime.Today;
            tb.video_paid = false;
            db.tbl_videos.InsertOnSubmit(tb);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

       // [CustomAuthorization_User(LoginPage = "~/Login/signin")]
        public ActionResult Details(Int16 id)
        {
            if (db.tbl_videos.Where(x => x.video_paid == false && x.video_id == id).Count() > 0)
            {
                tbl_view tbl = new tbl_view();
                tbl.view_date = DateTime.Now;
                tbl.video_id = id;
                tbl.login_id = SessionData.UserId;
                db.tbl_views.InsertOnSubmit(tbl);
                db.SubmitChanges();

                Model_video mdvideo = db.tbl_videos.Where(x => x.video_id == id).Select(x => new Model_video()
                {
                    video_id = x.video_id,
                    login_id = x.login_id,
                    video_name = x.video_name,
                    video_des = x.video_des,
                    video_path = x.video_path,
                    video_thumb = x.video_thumb,
                    video_date = x.video_date
                }).SingleOrDefault();
                ViewBag.totalview = db.tbl_views.Where(x => x.video_id == id).Count();
                ViewBag.videodate = mdvideo.video_date.ToShortDateString();
                List<Model_video> mv = new List<Model_video>();
                var query = (from v in db.tbl_videos
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
                             }).Take(4);

                foreach (var data in query)
                {
                    Model_video obj = new Model_video();
                    obj.totalview = data.TotalView;
                    obj.video_name = data.video_name;
                    obj.video_id = data.video_id;
                    obj.video_thumb = data.video_thumb;
                    obj.login_id = (DateTime.Today - data.video_date).Days;

                    mv.Add(obj);
                }
                Model_comment md = new Model_comment();
                md.login_id = SessionData.UserId;
                md.video_id = id;
                Model_video_page mdlvideo = new Model_video_page();
                mdlvideo.lsvideo = mv;
                mdlvideo.tbcomment = db.tbl_comments.Where(x => x.video_id == id).ToList();
                mdlvideo.mdvideo = mdvideo;
                mdlvideo.mdcomment = md;
                return View(mdlvideo);
            }
            else if (db.tbl_videos.Where(x => x.video_paid == true && x.video_id == id && SessionData.ustatus == "Free").Count() > 0)
            {
                return RedirectToAction("Buy");
            }
            else
            {
                tbl_view tbl = new tbl_view();
                tbl.view_date = DateTime.Now;
                tbl.video_id = id;
                tbl.login_id = SessionData.UserId;
                db.tbl_views.InsertOnSubmit(tbl);
                db.SubmitChanges();

                Model_video mdvideo = db.tbl_videos.Where(x => x.video_id == id).Select(x => new Model_video()
                {
                    video_id = x.video_id,
                    login_id = x.login_id,
                    video_name = x.video_name,
                    video_des = x.video_des,
                    video_path = x.video_path,
                    video_thumb = x.video_thumb,
                    video_date = x.video_date
                }).SingleOrDefault();
                ViewBag.totalview = db.tbl_views.Where(x => x.video_id == id).Count();
                ViewBag.videodate = mdvideo.video_date.ToShortDateString();
                List<Model_video> mv = new List<Model_video>();
                var query = (from v in db.tbl_videos
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
                             }).Take(4);

                foreach (var data in query)
                {
                    Model_video obj = new Model_video();
                    obj.totalview = data.TotalView;
                    obj.video_name = data.video_name;
                    obj.video_id = data.video_id;
                    obj.video_thumb = data.video_thumb;
                    obj.login_id = (DateTime.Today - data.video_date).Days;

                    mv.Add(obj);
                }
                Model_comment md = new Model_comment();
                md.login_id = SessionData.UserId;
                md.video_id = id;
                Model_video_page mdlvideo = new Model_video_page();
                mdlvideo.lsvideo = mv;
                mdlvideo.tbcomment = db.tbl_comments.Where(x => x.video_id == id).ToList();
                mdlvideo.mdvideo = mdvideo;
                mdlvideo.mdcomment = md;
                return View(mdlvideo);
            }
        }
        [HttpPost]
        public ActionResult Details(Model_video_page obj)
        {
            tbl_comment tbl = new tbl_comment();
            tbl.login_id = SessionData.UserId;
            tbl.video_id = obj.mdcomment.video_id;
            tbl.comment_des = obj.mdcomment.comment_des;
            tbl.comment_date = DateTime.Today;
            db.tbl_comments.InsertOnSubmit(tbl);
            db.SubmitChanges();
            return RedirectToAction("Details");
        }
        public ActionResult Delete(Int16 id)
        {
            Model_video obj = db.tbl_videos.Where(x => x.video_id == id).Select(x => new Model_video()
            {
                login_id = x.login_id,
                cat_id = x.cat_id,
                sub_cat_id = x.sub_cat_id,
                video_name = x.video_name,
                video_des = x.video_des,
                video_path = x.video_path
              }).SingleOrDefault();
            return View(obj);
        }
        [HttpPost]
        public ActionResult Delete(Int32 id)
        {
            tbl_video tb = db.tbl_videos.Where(x => x.video_id == id).Single<tbl_video>();
            db.tbl_videos.DeleteOnSubmit(tb);
            db.SubmitChanges();
        return RedirectToAction("Index");
        }

        public ActionResult Buy()
        {
            return View(); 
        }
        public ActionResult Loadsubcat(int id)
        {
            var data = from subcat in db.tbl_sub_categories
                       where subcat.cat_id == id
                       select subcat;
            return Json(new SelectList(data, "sub_cat_id", "sub_cat_name"));
        }
        public ActionResult Test(Int16 id)
        {
            tbl_view tbl = new tbl_view();
            tbl.view_date = DateTime.Now;
            tbl.video_id = id;
            tbl.login_id = SessionData.UserId;
            db.tbl_views.InsertOnSubmit(tbl);
            db.SubmitChanges();

            Model_video mdvideo = db.tbl_videos.Where(x => x.video_id == id).Select(x => new Model_video()
            {
                video_id = x.video_id,
                login_id = x.login_id,
                video_name = x.video_name,
                video_des = x.video_des,
                video_path = x.video_path,
                video_thumb = x.video_thumb,
                video_date = x.video_date
            }).SingleOrDefault();
            ViewBag.totalview = db.tbl_views.Where(x => x.video_id == id).Count();
            ViewBag.videodate = mdvideo.video_date.ToShortDateString();
            List<Model_video> mv = new List<Model_video>();
            var query = (from v in db.tbl_videos
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
                         }).Take(4);

            foreach (var data in query)
            {
                Model_video obj = new Model_video();
                obj.totalview = data.TotalView;
                obj.video_name = data.video_name;
                obj.video_id = data.video_id;
                obj.video_thumb = data.video_thumb;
                obj.login_id = (DateTime.Today - data.video_date).Days;
             
                mv.Add(obj);
            }
             Model_comment md = new Model_comment();
            md.login_id = SessionData.UserId;
            md.video_id = id;
            Model_video_page mdlvideo = new Model_video_page();
            mdlvideo.lsvideo = mv;
            mdlvideo.tbcomment = db.tbl_comments.Where(x => x.video_id == id).ToList();
            mdlvideo.mdvideo = mdvideo;
            mdlvideo.mdcomment = md;
            return View(mdlvideo);
        }
        [HttpPost]
        public ActionResult Test(Model_video_page obj)
        {
            tbl_comment tbl = new tbl_comment();
            tbl.login_id = SessionData.UserId;
            tbl.video_id = obj.mdcomment.video_id;
            tbl.comment_des = obj.mdcomment.comment_des;
            tbl.comment_date = DateTime.Today;
            db.tbl_comments.InsertOnSubmit(tbl);
            db.SubmitChanges();
            return RedirectToAction("Test");
        }
    }
}
