using System;
using System.Linq;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace MyRssReader.Models
{
    public class Rss
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PubDate { get; set; }
        public string ImageUrl { get; set; }
    }
}
