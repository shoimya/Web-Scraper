/* 
 CSE web Crawler/Scraper 
 @author: Shoimya Chowdhury
 @date: 2022/07/18
*/

using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSE_EbayTutorial_ 
{
    //386 is the number of instruments listed in DSE
    class DSE_Crawler 
    {
        public static char[] delims = new[] { '\r', '\n', '\t' };
        public static string[] Categories = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K","L","M","N","O","P","Q",
                                                   "R","S","T","U","V","W","X","Y","Z","Additional"};
        public static List<TBItem> GetDSEScrollerInfo()
        {
            List<TBItem> returnList = new List<TBItem>();
            string[] dels = new[] { "&nbsp;", "\r", "\n", "\t" };
            string url = "https://www.dsebd.org/index.php";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);

            //from scrollerinfo[0] you may extract the source code as innerhtml
            var scrollerinfo = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("scroll-item")).ToList();
            string scroller_item = scrollerinfo[0].InnerText.ToString();
            //string[] of properly split tickker bar data
            string[] splitScrollerItem = scroller_item.Replace(" ", "")
                .Split(dels, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < splitScrollerItem.Count(); i += 4)
            {
                TBItem listObj = new TBItem
                {
                    TickerName = splitScrollerItem[i],
                    LTP = splitScrollerItem[i + 1],
                    AbsoluteChange = splitScrollerItem[i + 2],
                    PercentageChange = splitScrollerItem[i + 3]
                };

                returnList.Add(listObj);
            }
            return returnList;
        }
        public static List<Holiday> GetDSEHolidays()
        {
            List<Holiday> returnList = new List<Holiday>();
            string url = "https://www.dsebd.org/hts.php";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);

            var tabledata = htmlDocument.DocumentNode.Descendants("table")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("table table-bordered background-white text-left")).ToList();
            // bellow array of strings has all the infotmation in string format thus you can skip the loop
            string[] tabletext = tabledata[0].InnerText.ToString()
                .Trim().Replace("  ","").Split(delims, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 4; i < tabletext.Count(); i += 4)
            {
                Holiday listObj = new Holiday();
                listObj.HolidayName = tabletext[i];
                listObj.Date = tabletext[i + 1];
                listObj.Day = tabletext[i + 2];
                listObj.NoOfDays = tabletext[i + 3];

                returnList.Add(listObj);
            }
            return returnList;
        }
        public static List<List<TopInstrument>> GetDSETOPTENGAINER()
        {
            List<TopInstrument> ClosePriceAndYCP = new List<TopInstrument>();
            List<TopInstrument> OpenPriceAndLTP = new List<TopInstrument>();
            List<List<TopInstrument>> returnList = new List<List<TopInstrument>>();

            var url = "https://www.dsebd.org/top_ten_gainer.php";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);

            var eachtable = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("table-responsive")).ToList();
            var ClosePrice_YCP = eachtable[0].InnerText.ToString()
                                .Split(delims, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 7; i < ClosePrice_YCP.Count()-2; i+=7)
            {
                var listObj = new TopInstrument();
                listObj.SerialNo = ClosePrice_YCP[i];
                listObj.TickerName = ClosePrice_YCP[i + 1];
                listObj.CloseP = ClosePrice_YCP[i + 2];
                listObj.High = ClosePrice_YCP[i + 3];
                listObj.Low = ClosePrice_YCP[i + 4];
                listObj.YCP = ClosePrice_YCP[i + 5];
                listObj.Change = ClosePrice_YCP[i + 6];
                ClosePriceAndYCP.Add(listObj);
            }
            var OpenPrice_LTP = eachtable[1].InnerText.ToString()
                                .Split(delims, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 7; i < OpenPrice_LTP.Count()-2; i+=7)
            {
                var listObj = new TopInstrument();
                listObj.SerialNo = ClosePrice_YCP[i];
                listObj.TickerName = ClosePrice_YCP[i + 1];
                listObj.OpenP = ClosePrice_YCP[i + 2];
                listObj.High = ClosePrice_YCP[i + 3];
                listObj.Low = ClosePrice_YCP[i + 4];
                listObj.LTP = ClosePrice_YCP[i + 5];
                listObj.Deviation = ClosePrice_YCP[i + 6];
                OpenPriceAndLTP.Add(listObj);
            }
            returnList.Add(ClosePriceAndYCP);
            returnList.Add(OpenPriceAndLTP);
            return returnList;
        }
        public static List<List<TopInstrument>> GetDSETOPTENLOSERS()
        {
            List<TopInstrument> ClosePriceAndYCP = new List<TopInstrument>();
            List<TopInstrument> OpenPriceAndLTP = new List<TopInstrument>();
            List<List<TopInstrument>> returnList = new List<List<TopInstrument>>();
            var url = "https://www.dsebd.org/top_ten_loser.php";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);
            var eachtable = htmlDocument.DocumentNode.Descendants("div")
                            .Where(node => node.GetAttributeValue("class", "")
                            .Equals("table-responsive")).ToList();
            var ClosePrice_YCP = eachtable[0].InnerText.ToString()
                                .Split(delims, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 7; i < ClosePrice_YCP.Count() - 2; i += 7)
            {
                var listObj = new TopInstrument();
                listObj.SerialNo = ClosePrice_YCP[i];
                listObj.TickerName = ClosePrice_YCP[i + 1];
                listObj.CloseP = ClosePrice_YCP[i + 2];
                listObj.High = ClosePrice_YCP[i + 3];
                listObj.Low = ClosePrice_YCP[i + 4];
                listObj.YCP = ClosePrice_YCP[i + 5];
                listObj.Change = ClosePrice_YCP[i + 6];
                ClosePriceAndYCP.Add(listObj);
            }
            var OpenPrice_LTP = eachtable[1].InnerText.ToString()
                               .Split(delims, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 7; i < OpenPrice_LTP.Count() - 2; i += 7)
            {
                var listObj = new TopInstrument();
                listObj.SerialNo = ClosePrice_YCP[i];
                listObj.TickerName = ClosePrice_YCP[i + 1];
                listObj.OpenP = ClosePrice_YCP[i + 2];
                listObj.High = ClosePrice_YCP[i + 3];
                listObj.Low = ClosePrice_YCP[i + 4];
                listObj.LTP = ClosePrice_YCP[i + 5];
                listObj.Deviation = ClosePrice_YCP[i + 6];
                OpenPriceAndLTP.Add(listObj);
            }
            returnList.Add(ClosePriceAndYCP);
            returnList.Add(OpenPriceAndLTP);
            return returnList;
        }
        public static List<Top30> GetDSEX()
        {
            //340 top30 objects will be returned 
            var returnList = new List<Top30>();
            var url = "https://www.dsebd.org/dseX_share.php";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);
            var eachtable = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("table-responsive")).ToList();
            
            var tabledata = eachtable[0].InnerText.ToString()
                            .Split(delims, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 11; i < tabledata.Count(); i += 11)
            {
                var listObj = new Top30();
                listObj.mOrderNumber = tabledata[i];
                listObj.mTickerName = tabledata[i + 1];
                listObj.mLTP = tabledata[i + 2];
                listObj.mHigh = tabledata[i + 3];
                listObj.mLow = tabledata[i + 4];
                listObj.mClose = tabledata[i + 5];
                listObj.mYCP = tabledata[i + 6];
                listObj.mRateChange = tabledata[i + 7];
                listObj.mTrade = tabledata[i + 8];
                listObj.mTradeValue = tabledata[i + 9];
                listObj.mTradeVolume = tabledata[i + 10];
                returnList.Add(listObj);
            }

            return returnList;
        }
        public static List<Top30> GetDSE30() 
        {
            List<Top30> returnList = new List<Top30>();

            var url = "https://www.dsebd.org/dse30_share.php";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);

            var tables = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("col-md-12 col-sm-12 col-xs-18")).ToList();

            var eachtable = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("table-responsive")).ToList();
            var tabledata = eachtable[0].InnerText.ToString()
                .Split(delims, StringSplitOptions.RemoveEmptyEntries);
            
            for (int i = 11; i < tabledata.Count()-2; i+=11)
            {
                var listObj = new Top30();
                listObj.mOrderNumber = tabledata[i];
                listObj.mTickerName = tabledata[i + 1];
                listObj.mLTP = tabledata[i + 2];
                listObj.mHigh = tabledata[i + 3];
                listObj.mLow = tabledata[i + 4];
                listObj.mClose = tabledata[i + 5];
                listObj.mYCP = tabledata[i + 6];
                listObj.mRateChange = tabledata[i + 7];
                listObj.mTrade = tabledata[i + 8];
                listObj.mTradeValue = tabledata[i + 9];
                listObj.mTradeVolume = tabledata[i + 10];
                returnList.Add(listObj);
            }

            return returnList;
        }
        public static List<DSECBItem> GetDSECircuitBreaker()
        {
            var returnlist = new List<DSECBItem>();
            char[] delims = new[] { '\r', '\n', '\t' };
            var url = "https://www.dse.com.bd/cbul.php";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);

            //bellow the inner HTML will give you the proper source code
            var trial = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("col-md-12 col-sm-12 col-12")).ToList();

            //Full list of individual entries, can take innerhtml or innertext
            var trial2 = trial[0].Descendants("tr").ToList();
            for (int i = 1; i < trial2.Count; i++)
            {
                var arrayofvals = trial2[i].InnerText.Replace("  ","").ToString().Split(delims, StringSplitOptions.RemoveEmptyEntries);
                var CbObj = new DSECBItem { SL = arrayofvals[0], TradeCode = arrayofvals[1], BreakerPercentage= arrayofvals[2]
                                            ,TickSize=arrayofvals[3],OpenAdjPrice=arrayofvals[4],LowerLimit=arrayofvals[5]
                                            ,UpperLimit = arrayofvals[6]};
                returnlist.Add(CbObj);
            }
            return returnlist;
        }
        public static void GetMarketPrice()
        {
            char[] delims = new[] { '\r', '\n', '\t' };

            var url = "https://www.dse.com.bd/latest_share_price_scroll_l.php";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);

            //table[0] contains the whole market price section and source code in innerhtml
            var table = htmlDocument.DocumentNode.Descendants("table")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("table table-bordered background-white shares-table fixedHeader")).ToList();

            // string[] of all the entries
            var arrayeachinstrument = table[0].InnerText.ToString().Replace("&nbsp;", "").Replace(" ", "")
                                    .Split(delims, StringSplitOptions.RemoveEmptyEntries);
        }
        public static void GetDSEMarketDepth()
        {
            var url = "https://www.dsebd.org/mkt_depth_3.php";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);

            //gets all the instruments in the tickerbar(Scroller) and data for each instrument
            var tables = htmlDocument.DocumentNode.Descendants("table").ToList();
            var trial = htmlDocument.DocumentNode.Descendants("div").Where(node => node.GetAttributeValue("id", "")
                .Equals("RightBody")).ToList();

            var trial1 = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("loadContent")).ToList();
            
            var trial2 = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("loadContent")).ToList();
            var trial3 = htmlDocument.DocumentNode.InnerHtml.ToString();


        }
        public static List<Company> GetDSEListedCompanies() 
        {
            var returnList = new List<Company>();

            var url = "https://www.dsebd.org/company_listing.php";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);

            var trial = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("row al-li")).ToList();

            //string[] of 652 intruments in DSE by Category
            var eachcompany = trial[0].InnerText.ToString().Replace(" ","")
                                .Replace("More...","")
                                .Split(delims, StringSplitOptions.RemoveEmptyEntries);
            string category = "";
            string tickerName = "";
            string companyName = "";

            foreach (var item in eachcompany)
            {
                if (category.Length != 0 && tickerName.Length != 0 && category[0] == tickerName[0] && companyName.Length != 0)
                {
                    var listItem = new Company
                    {
                        Category = category,
                        Tickername = tickerName,
                        CompanyName = companyName
                    };
                    returnList.Add(listItem);
                }

                if (category == "Additional" && tickerName.Length != 0 && companyName.Length != 0)
                {
                    var listItem = new Company
                    {
                        Category = category,
                        Tickername = tickerName,
                        CompanyName = companyName
                    };
                    returnList.Add(listItem);
                }

                if (item.Length == 1 || item == "Additional")
                {
                    category = item;
                }
                else
                {
                    var indexparenthesis = item.IndexOf("(");

                    var test = item.IndexOf("(",indexparenthesis+1);
                    if(test > -1 && test <=12 && test != indexparenthesis)
                    {
                        indexparenthesis = test;
                    }
                    var indexendparenthesis = item.IndexOf(")");
                    tickerName = item.Substring(0, indexparenthesis);
                    companyName = item.Substring(indexparenthesis);
                }
            }
            return returnList;
        }
        public static List<News> GetDSENews(string startDate, string endDate)
        {
            var returnList = new List<News>();
            var url = "https://www.dsebd.org/old_news.php?startDate=" + startDate + "&endDate="+endDate +"&criteria=4&archive=news";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);

            var allNews = htmlDocument.DocumentNode.Descendants("table")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("table-news")).ToList();
            //string[] of individual news from startdate to enddate 
            var newsdata = allNews[0].InnerText.Trim().ToString()
                .Replace("\t", "").Replace("\r", "").Replace("  ","").Replace("\n","")
                .Split("&nbsp;", StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < newsdata.Count(); i+=1)
            {
                string fullstring = newsdata[i];
                int index1 = fullstring.IndexOf("Trading Code:"); //+12
                int index2 = fullstring.IndexOf("News Title:"); //+10
                int index3 = fullstring.IndexOf("News:",index2); //+4
                int index4 = fullstring.IndexOf("Post Date:",index3); //9

                string code = fullstring.Substring(index1 + 13, index2);
                var trial1 = code.IndexOf("News Title:");
                var codeedited = code.Substring(0, trial1);

                string title = fullstring.Substring(index2 + 11, index3);
                var trial2 = title.IndexOf("News:");
                var titleedited = title.Substring(0, trial2);

                string news = fullstring.Substring(index3 + 5);
                string date = fullstring.Substring(index4 + 10);

                var newsObj = new News(date, titleedited, codeedited, news);
                returnList.Add(newsObj);

            }
            return returnList;
        }
        public static Company GetDSECompanyInfo(string tickerName)
        {
            Company returnCompany = new Company { Tickername = tickerName };
            var url = "https://www.dsebd.org/displayCompany.php?name="+ tickerName;
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);

            var table = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("col-md-12 col-sm-12 col-xs-18")).ToList();

            var eachTable = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("table-responsive")).ToList();

            returnCompany.mArrayBasicInfo = eachTable[2].InnerText.ToString()
                                .Replace("  ","")
                                .Split(delims, StringSplitOptions.RemoveEmptyEntries);

            var arrayotherinfoandSHP = eachTable[7].InnerText.ToString();
            returnCompany.mArrayFinancialPerformance = eachTable[5].InnerText.ToString()
                                            .Replace("  ", "")
                                            .Split(delims, StringSplitOptions.RemoveEmptyEntries);
            returnCompany.mArrayFinancialPerfContinued = eachTable[6].InnerText.ToString()
                                                    .Replace("  ", "")
                                                    .Split(delims, StringSplitOptions.RemoveEmptyEntries);

            var indexshareolders = arrayotherinfoandSHP.IndexOf("Share Holding Percentage");

            returnCompany.mArrayShareHoldingPercentage = arrayotherinfoandSHP.Substring(indexshareolders)
                                        .Replace("  ", "")
                                        .Split(delims, StringSplitOptions.RemoveEmptyEntries);
            returnCompany.mArrayOtherInfo = arrayotherinfoandSHP.Substring(0, indexshareolders)
                                    .Replace("  ", "")
                                    .Split(delims, StringSplitOptions.RemoveEmptyEntries);
            return returnCompany;
        }
    }
}