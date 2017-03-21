using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Video_Streaming.Models
{
    public class Model_view
    {
        public int view_id { get; set; }
        public int login_id { get; set; }
        public int video_id { get; set; }
        public DateTime view_date { get; set; }
    }
}