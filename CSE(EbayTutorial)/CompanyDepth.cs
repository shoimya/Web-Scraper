/* 
 CSE web Crawler/Scraper 
 @author: Shoimya Chowdhury
 @date: 2022/07/18
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE_EbayTutorial_
{
    public class CompanyDepth
    {
        public string mTickerName { get; set; }
        public string[] mBuyOrders { get; set; }
        public string[] mSellOrders { get; set; }
        public List<PriceComparison> mPriceComparison { get; set; }
        public string[] mFundamentalData { get; set; }
        public News mCompanyNews { get; set; }
        
    }
}
