/* 
 CSE web Crawler/Scraper 
 @author: Shoimya Chowdhury
 @date: 2022/07/17
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE_EbayTutorial_
{
    public class Company
    {
        public string Category { get; set; }
        public string CompanyName { get; set; }
        public string Tickername { get; set; }

        public string[] mArrayBasicInfo { get; set; }
        public string[] mArrayOtherInfo { get; set; }
        public string[] mArrayShareHoldingPercentage { get; set; }
        public string[] mArrayFinancialPerformance { get; set; }
        public string[] mArrayFinancialPerfContinued { get; set; }

        //public override string ToString() => $"{Category} {CompanyName} {Tickername}";


    }
}
