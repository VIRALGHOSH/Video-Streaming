using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Video_Streaming.Dbml;
using Video_Streaming.Models;

namespace Video_Streaming.Models
{
    public class Model_video_page
    {
        public Model_video mdvideo { get; set; }
        public Model_comment mdcomment { get; set; }
        public List<Model_video> lsvideo { get; set; }
        public IEnumerable<tbl_comment> tbcomment { get; set; }
    }
}