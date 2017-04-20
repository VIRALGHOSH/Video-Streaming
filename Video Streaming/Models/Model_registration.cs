using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Video_Streaming.Models
{
    public class Model_registration
    {
        public int reg_id { get; set; }
        [Required(ErrorMessage="Please enter your first name")]
        public string reg_fname { get; set; }
        [Required(ErrorMessage = "Please enter your last name")]
        public string reg_lname { get; set; }
        public string reg_photo { get; set; }
        public int login_id { get; set; }
        [Required(ErrorMessage = "Please select your gender")]
        public Boolean reg_gender { get; set; }
        public string reg_address { get; set; }
        [Required(ErrorMessage = "Please select your country")]
        public int country_id { get; set; }
        [Required(ErrorMessage = "Please select your state")]
        public int state_id { get; set; }
        [Required(ErrorMessage = "Please select your city")]
        public int city_id { get; set; }
       [RegularExpression("^[0-9]*$", ErrorMessage = "Mobile No must be numeric")]
       [Range(1001111111, 9999999999, ErrorMessage = "Please Enter a valid mobile no without country code (India:-9898455612)")]
        public decimal? reg_phno { get; set; }
    }
}