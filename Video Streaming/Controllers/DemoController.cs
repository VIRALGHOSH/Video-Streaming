using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Video_Streaming.Dbml;
using Video_Streaming.Models;
using NReco.VideoConverter;
using System.Net.Mail;
using Video_Streaming.Filters;
using System.Security.Cryptography;
using System.Text;

namespace Video_Streaming.Controllers
{
    public class DemoController : Controller
    {
        //
        // GET: /Demo/
        DataClasses1DataContext db = new DataClasses1DataContext();

      //  [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Index()
        {
            Model_video_page mdlvideo = new Model_video_page();
        
            mdlvideo.tbcomment = db.tbl_comments.ToList();
           
            return View(mdlvideo);
        }
        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Category()
        {


            return View();

        }
        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Viral()
       {
           try
           {
               MailMessage mail = new MailMessage();
               SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

               mail.From = new MailAddress("ghoshviral@gmail.com");
               mail.To.Add("viralghosh@gmail.com");
               mail.Subject = "Test Mail - 1";

               mail.IsBodyHtml = true;
               string htmlBody;

               htmlBody = "<h1>Write some HTML code here</h1>";

               mail.Body = htmlBody;

               SmtpServer.Port = 587;
               SmtpServer.Credentials = new System.Net.NetworkCredential("ghoshviral@gmail.com", "viral.com");
               SmtpServer.EnableSsl = true;

               SmtpServer.Send(mail);
           }
           catch (Exception ex)
           {
               Console.WriteLine(ex.ToString());
           }
         
            return View();
        }
        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult login()
        {

            return View();
        }
        //[CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult newuser(string obj)
        {

            ViewBag.xxxxx = keygen("vkjhtiovtjrv", "eruhrvvnvv@vv");

            return View();
        }

        public ActionResult video(int? id)
        {
            if (id == null)
            {
                return View(db.tbl_videos.ToList());
            }
            else
            {
                return View(db.tbl_videos.Where(x=>x.sub_cat_id==id).ToList());
            }
        }
        public ActionResult videotest()
        {
            ViewBag.videoby = "Viral Ghosh";
            ViewBag.videoname = "Video Testing";
            ViewBag.videodescription = "This is the Video Description ";
           
            return View();

        }
        public ActionResult upload()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult upload(Model_demo obj)
        {
             HttpPostedFileBase file = Request.Files["file1"];
            demo tb = new demo();
            var filename = Path.GetFileName(file.FileName);

            if (file.FileName != null)
            {
               
                string keyName = filename;
                int fileExtPos = keyName.LastIndexOf(".");
                if (fileExtPos >= 0)
                    keyName = keyName.Substring(0, fileExtPos);
                var path = Path.Combine(Server.MapPath("~/fileupload"), filename);
                file.SaveAs(path);
                string thumbnailJPEGpath = Server.MapPath("~/fileupload/" + keyName + ".jpg");
                FFMpegConverter ffmpeg = new FFMpegConverter();
                ffmpeg.GetVideoThumbnail(path, thumbnailJPEGpath, 2);
                var path2 = Path.Combine(Server.MapPath("~/fileupload"), keyName + ".jpg");
                S3Class s3obj = new S3Class();
                string str2 = s3obj.putObject("all.input.video.streaming", path, file.FileName.Replace(' ', '_'));
                string str = s3obj.putObject("transcoder.thumbnail.video.streaming", path2, keyName + ".jpg");
                tb.name = str;

            }


            db.demos.InsertOnSubmit(tb);
            db.SubmitChanges();
            return View();
        }
        public static Int64 keygen(string a, string b)
            {
                var x = a + b ;
                var hash = x.GetHashCode();
                return hash;
            }
            
        
        public ActionResult menu()
        {
            List<tbl_category> lscategory = db.tbl_categories.ToList();
            string str = "<li class='active'><span class='submenu-button'></span><a href='/Home'><i class='fi ion-ios-home'></i>Home</a> ";

            foreach (var item in lscategory)
            {
                List<tbl_sub_category> lssubcategory = db.tbl_sub_categories.Where(x=>x.cat_id==item.cat_id).ToList();
                if (lssubcategory.Count > 0)
                {
                    str = str + "<li class='has-sub'><span class='submenu-button'></span><a href='index'><i class='fi ion-ios-film-outline'></i>" + item.cat_name + "</a>";
                    str = str + "		   	  <ul>";
                    foreach (var item1 in lssubcategory)
                    {
                        str = str + "  	<li><a href='/home/sub_category/" + item1 .sub_cat_id+ "'>" + item1.sub_cat_name + "</a></li>";
                    }       
                    str = str + "  	  </ul>";
                    str = str + "  	</li>";
                }
                else
                {
                    str = str + "<li><a href='/home/category/" + item.cat_id + "'><i class='fi ion-ios-film-outline'></i>" + item.cat_name + "</a></li>";
                }
                //string str2 = "<li class='active'><span class='submenu-button'></span><a href='/Home/about'><i class='fi ion-ios-home'></i>About</a> ";

             }
            return Json(str);
        }


    }
}
