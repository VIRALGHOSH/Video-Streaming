using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Video_Streaming
{
    public class SessionData
    {

        public SessionData()
        {

        }

        public static int UserId
        {
            get
            {
                if (Convert.ToInt16(HttpContext.Current.Session["UserId"]) == 0)
                {
                    return 0;
                }
                return Convert.ToInt16(HttpContext.Current.Session["UserId"]);
            }
            set { HttpContext.Current.Session["UserId"] = value; }
        }
        public static int RegId
        {
            get
            {
                if (Convert.ToInt16(HttpContext.Current.Session["RegId"]) == 0)
                {
                    return 0;
                }
                return Convert.ToInt16(HttpContext.Current.Session["RegId"]);
            }
            set { HttpContext.Current.Session["RegId"] = value; }
        }


        public static int AdminId
        {
            get
            {
                if (Convert.ToInt16(HttpContext.Current.Session["AdminId"]) == 0)
                {
                    return 0;
                }
                return Convert.ToInt16(HttpContext.Current.Session["AdminId"]);
            }
            set { HttpContext.Current.Session["AdminId"] = value; }
        }

        public static string fname
        {
            get
            {
                if (Convert.ToString(HttpContext.Current.Session["fname"]) == "")
                {
                    HttpContext.Current.Session["fname"] = "Guest";
                }
                return Convert.ToString(HttpContext.Current.Session["fname"]);
            }
            set { HttpContext.Current.Session["fname"] = value; }
        }
        public static string ustatus
        {
            get
            {
                if (Convert.ToString(HttpContext.Current.Session["ustatus"]) == "")
                {
                    HttpContext.Current.Session["ustatus"] = "Free";
                }
                return Convert.ToString(HttpContext.Current.Session["ustatus"]);
            }
            set { HttpContext.Current.Session["ustatus"] = value; }
        }
        public static string currenturl
        {
            get
            {
                if (Convert.ToString(HttpContext.Current.Session["currenturl"]) == "")
                {
                    HttpContext.Current.Session["currenturl"] = "http://35.187.84.85/";
                }
                return Convert.ToString(HttpContext.Current.Session["currenturl"]);
            }
            set { HttpContext.Current.Session["currenturl"] = value; }
        }

      public static string photo
        {
            get
            {
                if (Convert.ToString(HttpContext.Current.Session["photo"]) == "")
                {
                    HttpContext.Current.Session["photo"] = "https://ssl.gstatic.com/accounts/ui/avatar_2x.png";
                }
                return Convert.ToString(HttpContext.Current.Session["photo"]);
            }
            set { HttpContext.Current.Session["photo"] = value; }
        }

    }
}