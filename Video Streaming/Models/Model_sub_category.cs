using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Video_Streaming.Models
{
    public class Model_sub_category
    {
        public int sub_cat_id { get; set; }
        public int? cat_id { get; set; }
        public string sub_cat_name { get; set; }
        public string sub_cat_des { get; set; }

    }
}