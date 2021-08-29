using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using HtmlAgilityPack;

namespace MyRssReader.Models
{
    
    public class RssReader
    {
        //This method extracts the image from the <description> tag
        static string ImgSrc(string content)
        {
            Regex searchImg = new Regex("<img.*?>");

            Debug.WriteLine("item");
            string firstResult = "";

            foreach (Match m in searchImg.Matches(content))
            {
                Regex searchSrc = new Regex("(?<=src=\").*?(?=\")");
                string result = searchSrc.Match(m.ToString()).ToString();
                if (firstResult == "")
                    firstResult = result;
                Debug.WriteLine(result);
            }

            return firstResult;
        }

        //This method extracts the paragraph from the <description> tag
        public static string ParseParagraph(string input)
        {
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(input);
            var para = html.DocumentNode.Descendants("div").FirstOrDefault();
            if (para != null)
            {
                return para.InnerText;
            }

            return null;
        }

        //This method parses the url to display the relevant information
        //private static string _blogURL = "https://rss.app/feeds/C5jcrhFyIA8R7HC9.xml";
        public static IEnumerable<Rss> GetRssFeed(string url)
        {
            XDocument feedXml = XDocument.Load(url);
            //XDocument feedXml = XDocument.Load(url);
            var feeds = from feed in feedXml.Descendants("item")

                        select new Rss
                        {
                            Title = feed.Element("title").Value,
                            Link = feed.Element("link").Value,
                            Description = ParseParagraph(feed.Element("description").Value),
                            PubDate = feed.Element("pubDate").Value,
                            ImageUrl = ImgSrc(feed.Element("description").Value)
                        };

            return feeds;
        }

        //This method concatenates multiple url's
        public static IEnumerable<Rss> GetMultipleFeeds(string searchTerm)
        {
            string url1 = "https://rss.app/feeds/W46Ct8KW1UHV5xHF.xml";
            string url2 = "https://rss.app/feeds/dtz5Wi9uxMOtXnZ3.xml";

            return GetRssFeed(url1).Union(GetRssFeed(url2));
        }
    }
}
