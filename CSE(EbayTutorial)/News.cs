/* 
 CSE web Crawler/Scraper 
 @author: Shoimya Chowdhury
 @date: 2022/07/19
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE_EbayTutorial_
{
    public class News
    {
        string Date { get; set; }
        string Title { get; set; }
        string Classification { get; set; }
        string Content { get; set; }

        public News(string date, string title, string classification, string content)
        {
            this.Date = date;
            this.Title = title;
            this.Classification = classification;
            this.Content = content;
        }
        public override string ToString()
        {
            return $"{Date} : {Title}\n{Classification} : {Content}";
        }
    }
}
