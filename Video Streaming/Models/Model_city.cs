using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Video_Streaming.Models
{
    public class Model_city
    {
        public int city_id { get; set; }
        [Required(ErrorMessage = "Please Select a State ")]
        public int state_id { get; set; }
        [Required(ErrorMessage = "Please Select a Country ")]
        public int country_id { get; set; }
        [Required(ErrorMessage = "Please Enter a City name")]
        public string city_name { get; set; }
    }
}