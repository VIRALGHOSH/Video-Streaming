using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Video_Streaming.Models
{
    public class Model_comment
    {
        public int comment_id { get; set; }
        public int login_id { get; set; }
        public int video_id { get; set; }
        public string comment_des { get; set; }
        public DateTime comment_date { get; set; }
        public int datediff { get; set; }
    }
}