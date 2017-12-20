using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class ImageLibraries
    {
        public int id { get; set; }
        public List<Image> listImage { get; set; }
    }
    public class Image
    {
        public string image { get; set; }
    }
}