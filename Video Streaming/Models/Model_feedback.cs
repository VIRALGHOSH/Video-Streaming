using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Video_Streaming.Models
{
    public class Model_feedback
    {
        public int f_id { get; set; }
        public int login_id { get; set; }
        [Required(ErrorMessage = "Please Enter Your Name")]
        public string f_name { get; set; }
        public string f_email { get; set; }
        [Required(ErrorMessage = "Please Enter Your Feedback")]
        public string f_des { get; set; }
        public Boolean f_status { get; set; }
        public DateTime f_date { get; set; }

    }
}