using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Video_Streaming.Models
{
    public class Model_complain
    {
        public int com_id { get; set; }
        public int login_id { get; set; }
        [Required(ErrorMessage = "Please Enter Your Complain Subject")]
        public string com_sub { get; set; }
        [Required(ErrorMessage = "Please Enter Your Comlain Description")]
        public string com_des { get; set; }
        public Boolean com_status { get; set; }
        public DateTime com_date { get; set; }

    }
}