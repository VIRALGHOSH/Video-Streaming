using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Video_Streaming.Models
{
    public class Model_subscription
    {
        public int sub_id { get; set; }
        public int login_id { get; set; }
        public DateTime sub_pay_date { get; set; }
        public DateTime sub_end_date { get; set; }
        public int sub_amt { get; set; }
    }
}