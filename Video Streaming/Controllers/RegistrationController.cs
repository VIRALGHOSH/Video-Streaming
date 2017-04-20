using Amazon.S3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Video_Streaming.Dbml;
using Video_Streaming.Filters;
using Video_Streaming.Models;

namespace Video_Streaming.Controllers
{
    public class RegistrationController : Controller
    {
        //
        // GET: /Registration/
        DataClasses1DataContext db = new DataClasses1DataContext();
        public ActionResult Index()
        {
            return View(db.tbl_registrations.ToList());
        }
        public ActionResult Signup()
        {
            ViewBag.genderid = new SelectList(db.tbl_genders.ToList(), "gen_id", "gen_name");
            ViewBag.countryid = new SelectList(db.tbl_countries.ToList(), "country_id", "country_name");
            ViewBag.stateid = new SelectList(db.tbl_states.ToList(), "state_id", "state_name");
            ViewBag.cityid = new SelectList(db.tbl_cities.ToList(), "city_id", "city_name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(Model_Login_Register obj)
        {
            if (db.tbl_logins.Where(x => x.email == obj.mdlogin.email).Count() == 0)
            {
                string host = Request.Url.Authority;
                string key = GetUniqueKey(64);
                string sendurl = "http://" + host + "/login/Activate/"+ key;
                SendMail(obj.mdlogin.email,obj.mdregister.reg_fname, "Confirm Registration Email", sendurl);

                HttpPostedFileBase file = Request.Files["file1"];
                tbl_login tbl = new tbl_login();
                tbl.email = obj.mdlogin.email;
                tbl.password = obj.mdlogin.password;
                tbl.log_status = key;
                tbl.log_date = DateTime.Today;
                db.tbl_logins.InsertOnSubmit(tbl);
                db.SubmitChanges();


                tbl_registration tb = new tbl_registration();

                if (file.FileName != "")
                {
                    var filename = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/fileupload"), filename);
                    file.SaveAs(path);
                    S3Class s3obj = new S3Class();
                    string str = s3obj.putObject("all.input.video.streaming", path, file.FileName.Replace(' ', '_'));
                    tb.reg_photo = str;
                }

                tb.reg_fname = obj.mdregister.reg_fname;
                tb.reg_lname = obj.mdregister.reg_lname;
                tb.reg_gender = obj.mdregister.reg_gender;
                tb.reg_address = obj.mdregister.reg_address;
                tb.country_id = obj.mdregister.country_id;
                tb.state_id = obj.mdregister.state_id;
                tb.city_id = obj.mdregister.city_id;
                tb.reg_phno = obj.mdregister.reg_phno;
                tb.login_id = tbl.login_id;
                db.tbl_registrations.InsertOnSubmit(tb);
                db.SubmitChanges();

                //string host = Request.Url.Authority;
                //host = host + GetUniqueKey(64);
                //string sendurl = "http://" + host;
                //SendMail(obj.mdlogin.email, "Confirm Email", sendurl);
                return RedirectToAction("Confirm");
            }
            else
            {
                ModelState.AddModelError("error", "Error! Your email is already registered with us");

                ViewBag.genderid = new SelectList(db.tbl_genders.ToList(), "gen_id", "gen_name");
                ViewBag.countryid = new SelectList(db.tbl_countries.ToList(), "country_id", "country_name");
                ViewBag.stateid = new SelectList(db.tbl_states.ToList(), "state_id", "state_name");
                ViewBag.cityid = new SelectList(db.tbl_cities.ToList(), "city_id", "city_name");
                return View("SignUp");

            }
        }
       
        public ActionResult Edit(int id)
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
                email = x.email,
                password = x.password
            }).SingleOrDefault();
            Model_Login_Register mdlgrg = new Model_Login_Register();

            mdlgrg.mdregister = obj;
            mdlgrg.mdlogin = obj2;
            return View(mdlgrg);
        }
        [CustomAuthorization_User(LoginPage = "~/Login/signin")]
        [HttpPost]
        public ActionResult Edit(Model_Login_Register obj)
        {
            HttpPostedFileBase file = Request.Files["file1"];
            tbl_login tbl = db.tbl_logins.Where(x => x.login_id == obj.mdlogin.login_id).Single<tbl_login>();
            tbl.email = obj.mdlogin.email;
            tbl.password = obj.mdlogin.password;
            db.SubmitChanges();

            tbl_registration tb = db.tbl_registrations.Where(x => x.reg_id == obj.mdregister.reg_id).Single<tbl_registration>();
            if (file.FileName != "")
            {
                var filename = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/fileupload"), filename);
                file.SaveAs(path);
                S3Class s3obj = new S3Class();
                string str = s3obj.putObject("all.input.video.streaming", path, file.FileName.Replace(' ', '_'));
                tb.reg_photo = str;
                SessionData.photo = str;
            }

            tb.reg_fname = obj.mdregister.reg_fname;
            tb.reg_lname = obj.mdregister.reg_lname;
            tb.reg_address = obj.mdregister.reg_address;
            tb.country_id = obj.mdregister.country_id;
            tb.state_id = obj.mdregister.state_id;
            tb.city_id = obj.mdregister.city_id;
            tb.reg_phno = obj.mdregister.reg_phno;
            tb.login_id = tbl.login_id;
            db.SubmitChanges();
            return RedirectToAction("Index", "Home");

        }
        public ActionResult Details(Int16 id)
        {
            Model_registration obj = db.tbl_registrations.Where(x => x.reg_id == id).Select(x => new Model_registration()
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

            Model_login obj2 = db.tbl_logins.Where(x => x.login_id == obj.login_id).Select(x => new Model_login()
            {
                login_id = x.login_id,
                email = x.email,
                password = x.password
            }).SingleOrDefault();
            Model_Login_Register mdlgrg = new Model_Login_Register();

            mdlgrg.mdregister = obj;
            mdlgrg.mdlogin = obj2;
            return View(mdlgrg);
        }

        public ActionResult Delete(Int16 id)
        {
            Model_registration obj = db.tbl_registrations.Where(x => x.reg_id == id).Select(x => new Model_registration()
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
                reg_phno = x.reg_phno
            }).SingleOrDefault();
            ViewBag.countryid = new SelectList(db.tbl_countries.ToList(), "country_id", "country_name", obj.country_id);
            ViewBag.stateid = new SelectList(db.tbl_states.ToList(), "state_id", "state_name", obj.state_id);
            ViewBag.cityid = new SelectList(db.tbl_cities.ToList(), "city_id", "city_name", obj.city_id);
            ViewBag.genderid = new SelectList(db.tbl_genders.ToList(), "gen_id", "gen_name", obj.reg_gender);

            Model_login obj2 = db.tbl_logins.Where(x => x.login_id == obj.login_id).Select(x => new Model_login()
            {
                login_id = x.login_id,
                email = x.email,
                password = x.password
            }).SingleOrDefault();
            Model_Login_Register mdlgrg = new Model_Login_Register();

            mdlgrg.mdregister = obj;
            mdlgrg.mdlogin = obj2;
            return View(mdlgrg);
        }
        [HttpPost]
        public ActionResult Delete(Int32 id)
        {
            tbl_registration tb = db.tbl_registrations.Where(x => x.reg_id == id).Single<tbl_registration>();
            tbl_login tbl = db.tbl_logins.Where(x => x.login_id == tb.login_id).Single<tbl_login>();
            db.tbl_logins.DeleteOnSubmit(tbl);
            db.tbl_registrations.DeleteOnSubmit(tb);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Resend()
        { 
            return View(); 
        }
        [HttpPost]
        public ActionResult Resend(Model_login obj)
        {
            if (db.tbl_logins.Where(x => x.email == obj.email && x.log_status == "Active").Count() > 0)
            {
                ModelState.AddModelError("error", "Your Email ID is already Activated Please Sign In");
                return View();
            }
            else if (db.tbl_logins.Where(x => x.email == obj.email).Count() > 0)
            {
                tbl_login tbl = db.tbl_logins.Where(x => x.email == obj.email).Single<tbl_login>();
                tbl_registration tbl2 = db.tbl_registrations.Where(x => x.login_id == tbl.login_id).Single<tbl_registration>();

                string host = Request.Url.Authority;
                string key = GetUniqueKey(64);
                string sendurl = "http://" + host + "/login/Activate/" + key;
                SendMail(obj.email,tbl2.reg_fname, "Confirm Registration Email", sendurl);
                tbl.log_status = key;
                db.SubmitChanges();
                ModelState.AddModelError("error", "Activation Email Sent");
                return View();
            }
            else
            {
                ModelState.AddModelError("error", "Email ID not registered Plz register First");

                return View();
            }
        }

        public ActionResult Loadstate(int id)
        {
            var data = from states in db.tbl_states
                       where states.country_id == id
                       select states;
            return Json(new SelectList(data, "state_id", "state_name"));
        }
        public ActionResult Loadcity(int id)
        {
            var data = from cites in db.tbl_cities
                       where cites.state_id == id
                       select cites;
            return Json(new SelectList(data, "city_id", "city_name"));
        }
        public static void SendMail(string strTo, string strfname, string strSubject, string strBody)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("ghoshviral@gmail.com");
                mail.To.Add(strTo);
                mail.Subject = " MyTube -" + strSubject;
                mail.IsBodyHtml = true;
                string htmlBody;
                htmlBody = "Dear + strfname +,<br /> we received a request for Account Creation <br /><a href='" + strBody + "' target='_blank'>Go to this page</a> to Activate your Account<br /><br />Regards,<br />The MyTube team<hr />If you are unable to clink on the link please copy paste this url on your broser <br /> " + strBody;
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
        public ActionResult Confirm()
        {
            return View(); 
        }
        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
    }
}
