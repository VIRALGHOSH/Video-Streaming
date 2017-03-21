using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Video_Streaming.Models
{
    public class Model_video
    {
        public int video_id { get; set; }
        public int login_id { get; set; }
        [Required(ErrorMessage = "Please Enter Your Category Name")]
        public int cat_id { get; set; }
        [Required(ErrorMessage = "Please Select Your Sub Category Name")]
        public int sub_cat_id { get; set; }
        [Required(ErrorMessage = "Please Enter Your Video Name")]
        public string video_name { get; set; }
        [Required(ErrorMessage = "Please Enter Your Video Description")]
        public string video_des { get; set; }
        public string video_path { get; set; }
        public string video_thumb { get; set; }
        public DateTime video_date { get; set; }
        public int totalview { get; set; }
        public bool video_paid { get; set; }
    }
}