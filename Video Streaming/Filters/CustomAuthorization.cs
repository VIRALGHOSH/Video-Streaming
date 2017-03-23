using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Video_Streaming.Filters
{
    public class CustomAuthorization_Admin : AuthorizeAttribute
    {
        public string LoginPage { get; set; }
        
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
           
            if (SessionData.AdminId == 0)
            {
                SessionData.currenturl = HttpContext.Current.Request.Url.AbsoluteUri; 
                filterContext.HttpContext.Response.Redirect(LoginPage);
            }
        }
    }

    public class CustomAuthorization_User : AuthorizeAttribute
    {
        public string LoginPage { get; set; }
       
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
           
            if (SessionData.UserId == 0)
            {
                SessionData.currenturl = HttpContext.Current.Request.Url.AbsoluteUri; 
                filterContext.HttpContext.Response.Redirect(LoginPage);
            }
        }
    }
}
