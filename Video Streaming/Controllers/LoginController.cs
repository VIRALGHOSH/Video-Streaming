using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using Video_Streaming.Dbml;
using Video_Streaming.Filters;
using Video_Streaming.Models;

namespace Video_Streaming.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        DataClasses1DataContext db = new DataClasses1DataContext();
        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Index()
        {
            return View(db.tbl_logins.ToList());
        }
        [CustomAuthorization_Admin(LoginPage = "/admin/signin")]
        public ActionResult Details(int id)
        {
            Model_registration obj = db.tbl_registrations.Where(x => x.login_id == id).Select(x => new Model_registration()
            {
                reg_id = x.reg_id,
                reg_fname = x.reg_fname,
                reg_lname = x.reg_lname,
                login_id = x.login_id,
                reg_photo = x.reg_photo,
                reg_address = x.reg_address,
                country_id = x.country_id,
                state_id = x.state_id,
                city_id = x.city_id,
                reg_phno = x.reg_phno,
                reg_gender = x.reg_gender
            }).SingleOrDefault();
            ViewBag.countryid = new SelectList(db.tbl_countries.ToList(), "country_id", "country_name", obj.country_id);
            ViewBag.stateid = new SelectList(db.tbl_states.ToList(), "state_id", "state_name", obj.state_id);
            ViewBag.cityid = new SelectList(db.tbl_cities.ToList(), "city_id", "city_name", obj.city_id);
            ViewBag.genderid = new SelectList(db.tbl_genders.ToList(), "gen_id", "gen_name", obj.reg_gender);

            Model_login obj2 = db.tbl_logins.Where(x => x.login_id == obj.login_id).Select(x => new Model_login()
            {
                login_id = x.login_id,
                email = x.email
            }).SingleOrDefault();
            Model_Login_Register mdlgrg = new Model_Login_Register();
            mdlgrg.mdregister = obj;
            mdlgrg.mdlogin = obj2;
            return View(mdlgrg);
        }
        public ActionResult Signin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signin(Model_login obj)
        {
            if(db.tbl_logins.Where(x => x.email == obj.email && x.password == obj.password).Count() > 0)
            {
                if (db.tbl_logins.Where(x => x.email == obj.email && x.password == obj.password && x.log_status == "Active").Count() > 0)
                {
                    tbl_login tbl = db.tbl_logins.Where(x => x.email == obj.email && x.password == obj.password).Single<tbl_login>();
                    tbl_registration tb = db.tbl_registrations.Where(x => x.login_id == tbl.login_id).Single<tbl_registration>();
                    SessionData.fname = tb.reg_fname;
                    SessionData.UserId = tbl.login_id;
                    SessionData.photo = tb.reg_photo;
                    SessionData.RegId = tb.reg_id;
                    if (db.tbl_subscriptions.Where(x => x.sub_end_date > DateTime.Now && x.login_id == tbl.login_id).Count() > 0)
                    {
                        SessionData.ustatus = "Paid";
                    }
                    return Redirect(SessionData.currenturl);
                }
                else
                {
                    ModelState.AddModelError("error", "Error! Please check your Email to Activate Your Account");
                    return View();
                }
                
            }
            else { 
            ModelState.AddModelError("error", "Error! Please check your Username And/or Password");
            return View();
            }
            }
        public ActionResult Admin()
        {
            return View();
        }
           public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Signin");
        }
           public ActionResult Forgot()
           {

               return View();
           }
           [HttpPost]
           public ActionResult Forgot(Model_login obj)
           {
               if (db.tbl_logins.Where(x => x.email == obj.email).Count() > 0)
               {
                   tbl_login tbl = db.tbl_logins.Where(x => x.email == obj.email).Single<tbl_login>();

                   SendMail(obj.email, "Forgot Password", "Your password : " + tbl.password);

                   ModelState.AddModelError("sucess", "Email Sucessfully sent, check your Email address");
                   return View();
               }
               else
               {
                
                   ModelState.AddModelError("error", "Error! Please check your Email address");
                   return View();
               }

           }
           public ActionResult Activate()
           {
               string host = Request.Url.PathAndQuery;
               host = host.Replace("/login/Activate/", "");
               if (db.tbl_logins.Where(x => x.log_status == host).Count() > 0)
               {
                   Model_login log = db.tbl_logins.Where(x => x.log_status == host).Select(x => new Model_login()
                 {
                     login_id = x.login_id,
                     email = x.email
                 }).SingleOrDefault();
                   return View(log);
               }
               ModelState.AddModelError("error", "Invalid Activation Url or Account Already Activated");
                   return View();
           }
        [HttpPost]
           public ActionResult Activate(Model_login obj)
           {
                if (db.tbl_logins.Where(x => x.login_id == obj.login_id && x.log_status == "Active").Count() > 0)
                   {
                       ModelState.AddModelError("error", "Your Email ID is already Activated");
                       return RedirectToAction("Signin");
                   }
                   else
                   { 
                    if(obj.login_id != 0){
                        tbl_login tbl = db.tbl_logins.Where(x => x.login_id == obj.login_id).Single<tbl_login>();
                        tbl.log_status = "Active";
                        db.SubmitChanges();
                        return RedirectToAction("SignIn");
                    }
                    ModelState.AddModelError("error", "Invalid Activation Url");
                       
                       return View();
                   }
           
           }
           public static void SendMail(string strTo, string strSubject, string strBody)
           {
               try
               {
                   MailMessage mail = new MailMessage();
                   SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                   mail.From = new MailAddress("ghoshviral@gmail.com");
                   mail.To.Add(strTo);
                   mail.Subject = " MyTube - " + strSubject;
                  
                   mail.IsBodyHtml = true;
                   string htmlBody;
                   htmlBody = "Dear User,<br /> We received a request for Password recovery <br />" + strBody + "<br /><a href='http://35.187.84.85/login/Signin' target='_blank'>Click Here</a> to Login into your Account<br /><br />Regards,<br />The MyTube team<hr />";
                   //htmlBody = strBody;
                   mail.Body = htmlBody;
                   SmtpServer.Port = 587;
                   SmtpServer.Credentials = new System.Net.NetworkCredential("ghoshviral@gmail.com", "viral.com");
                   SmtpServer.EnableSsl = true;
                   SmtpServer.Send(mail);
               }
               catch (Exception ex)
               {
                               
               }
          
           }
    }
}
