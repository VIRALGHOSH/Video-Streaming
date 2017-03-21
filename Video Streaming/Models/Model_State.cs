using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Video_Streaming.Models
{
    public class Model_State
    {
        public int state_id { get; set; }
        public int? country_id { get; set; }
        public string state_name { get; set; }
    }
}