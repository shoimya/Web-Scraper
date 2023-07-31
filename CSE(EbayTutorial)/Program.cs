/* 
 CSE web Crawler/Scraper 
 @author: Shoimya Chowdhury
 @date: 2022/07/18
*/

using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CSE_EbayTutorial_
{   
    class Program
    {
        public static char[] delims = new[] { '\r', '\n', '\t' };
        static void Main(string[] args)
        {
            List<Company> test = new List<Company>();

            GetListOfCompanies(test);

            Console.ReadLine();
        }


        public static void GetCSEScrollerInfo()
        {
            string url = "https://www.cse.com.bd";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);

            var scrollerinfo = htmlDocument.DocumentNode.Descendants("header").ToList();

            var x =  htmlDocument.DocumentNode.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("frame-area")).ToList();
            var y = htmlDocument.DocumentNode.Descendants("div").ToList();
        }
        public static void GetCSETickerInfo() //using market price
        {
            string url = "https://www.cse.com.bd/market/current_price";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);

            var x = htmlDocument.DocumentNode.Descendants("table")
                .Where(node => node.GetAttributeValue("id", "")
                .Equals("dataTable")).ToList();
            string[] collection = x[0].InnerText.ToString().Trim().Split(delims);
            collection = collection.Where(x => !string.IsNullOrEmpty(x)).ToArray();

        }
        public static List<Holiday> GetCDBLHoliday()
        {
            List<Holiday> listHoliday = new List<Holiday>();
            string url = "https://www.cdbl.com.bd/holidays.php";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);
            //tables[0] contains full source html in innerhtml
            var tables = htmlDocument.DocumentNode.Descendants("table")
                .Where(node => node.GetAttributeValue("id", "")
                .Equals("tblist")).ToList();
            string[] tabledata = tables[0].InnerText.ToString()
                .Replace("  ","")
                .Split(delims, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 5; i < tabledata.Count(); i+=5)
            {
                Holiday listObj = new Holiday();
                listObj.Serial = tabledata[i];
                listObj.HolidayName = tabledata[i + 1];
                listObj.Date = tabledata[i + 2];
                listObj.Day = tabledata[i + 3];
                listObj.NoOfDays = tabledata[i + 4];
                listHoliday.Add(listObj);
            }
            return listHoliday;
        }
        public static List<SectorIndex> GetCSESectorWiseIndex()
        {
            List<SectorIndex> returnList = new List<SectorIndex>();
            var url = "https://www.cse.com.bd/market/sectorindexdata";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);
            var tables = htmlDocument.DocumentNode.Descendants("div")
                            .Where(node => node.GetAttributeValue("id", "")
                            .Equals("top_content_7")).ToList();
            
            // sectorwiseindex[0] contains the source html in inner html
            var sectorwiseindex = tables[0].Descendants("table")
                                .Where(node => node.GetAttributeValue("id", "")
                                .Equals("dataTablex")).ToList();
            var tabledata = sectorwiseindex[0].InnerText.ToString()
                    .Split(delims, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 6; i < tabledata.Count(); i+=6)
            {
                var listObj = new SectorIndex();
                listObj.Serial = tabledata[i];
                listObj.SectorName = tabledata[i + 1];
                listObj.IndexValue = tabledata[i + 2];
                listObj.PreviousDayIndex = tabledata[i + 3];
                listObj.AbsoluteChange = tabledata[i + 4];
                listObj.PercentageChange = tabledata[i + 5];
                returnList.Add(listObj);
            }
            return returnList;
        }
        public static List<TopInstrument> GetCSETopTenGainers()
        {
            List<TopInstrument> returnList = new List<TopInstrument>();
            var url = "https://www.cse.com.bd/";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);

            var csetopten = htmlDocument.DocumentNode.Descendants("div")
                            .Where(node => node.GetAttributeValue("id", "")
                            .Equals("content_1")).ToList();
            var tabledata = csetopten[0].InnerText.ToString()
                            .Split(delims, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 4; i < tabledata.Count(); i += 4)
            {
                var listObj = new TopInstrument();
                listObj.TickerName = tabledata[i];
                listObj.LTP = tabledata[i + 1];
                listObj.AbsoluteChange = tabledata[i + 2];
                listObj.Change = tabledata[i + 3];
                returnList.Add(listObj);
            }
            return returnList;
        }
        public static List<TopInstrument> GetCSETopTenLosers()
        {
            List<TopInstrument> returnList = new List<TopInstrument>();

            var url = "https://www.cse.com.bd/";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);

            var csetopten = htmlDocument.DocumentNode.Descendants("div")
                            .Where(node => node.GetAttributeValue("id", "")
                            .Equals("content_2")).ToList();
            var tabledata = csetopten[0].InnerText.ToString()
                            .Split(delims, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 4; i < tabledata.Count(); i+=4)
            {
                var listObj = new TopInstrument();
                listObj.TickerName = tabledata[i];
                listObj.LTP = tabledata[i + 1];
                listObj.AbsoluteChange = tabledata[i + 2];
                listObj.Change = tabledata[i + 3];
                returnList.Add(listObj);
            }
            return returnList;
        }
        public static List<Top30> GetCSEX()
        {
            List<Top30> returnList = new List<Top30>();
            var url = "https://www.cse.com.bd/market/sectorindexdata";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);
            var tables = htmlDocument.DocumentNode.Descendants("div")
                            .Where(node => node.GetAttributeValue("id", "")
                            .Equals("top_content_4")).ToList();
            //top30[1] has all the data
            var csex = tables[0].Descendants("table")
                            .Where(node => node.GetAttributeValue("id", "")
                            .Equals("dataTablex")).ToList();
            var tabledata = csex[1].InnerText.ToString()
                                .Split(delims, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 11; i < tabledata.Count(); i += 10)
            {
                //these instruments are listed but are empty in most fields causes errors
                if (tabledata[i + 1] == "TAUFIKA" || tabledata[i + 1] == "MICEMENT")
                {
                    var listObj1 = new Top30();
                    listObj1.mOrderNumber = tabledata[i];
                    listObj1.mTickerName = tabledata[i + 1];
                    returnList.Add(listObj1);
                    i = i + 3;
                }
                var listObj = new Top30();
                listObj.mOrderNumber = tabledata[i];
                listObj.mTickerName = tabledata[i + 1];
                listObj.mLTP = tabledata[i + 2];
                listObj.mOpen = tabledata[i + 3];
                listObj.mHigh = tabledata[i + 4];
                listObj.mLow = tabledata[i + 5];
                listObj.mYCP = tabledata[i + 6];
                listObj.mTrade = tabledata[i + 7];
                listObj.mTradeValue = tabledata[i + 8];
                listObj.mTradeVolume = tabledata[i + 9];
                returnList.Add(listObj);
            }
            return returnList;
        }
        public static List<Top30> GetCSE30()
        {
            List<Top30> returnList = new List<Top30>();
            var url = "https://www.cse.com.bd/market/sectorindexdata";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);

            var tables = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("id", "")
                .Equals("top_content_2")).ToList();
            //top30[1] has all the data
            var top30 = tables[0].Descendants("table")
                .Where(node => node.GetAttributeValue("id", "")
                .Equals("dataTablex")).ToList();
            var tabledata = top30[1].InnerText.ToString()
                .Split(delims, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 11; i < tabledata.Count(); i+=10)
            {
                var listObj = new Top30();
                listObj.mOrderNumber = tabledata[i];
                listObj.mTickerName = tabledata[i + 1];
                listObj.mLTP = tabledata[i + 2];
                listObj.mOpen = tabledata[i + 3];
                listObj.mHigh = tabledata[i + 4];
                listObj.mLow = tabledata[i + 5];
                listObj.mYCP = tabledata[i + 6];
                listObj.mTrade = tabledata[i + 7];
                listObj.mTradeValue = tabledata[i + 8];
                listObj.mTradeVolume = tabledata[i + 9];
                returnList.Add(listObj);
            }
            return returnList;
        }

        /// <summary>
        /// WILL BE USED BASED ON SEARCH.
        /// This fucntion is used to get the depth information such as News, Buy orders,
        /// Sell Orders, Price Comparison and fundamental data of instruments.
        /// </summary>
        /// <param name="ticker"></param>
        /// <return> CompanyDepth object </return>
        private static CompanyDepth GetTickerDepth(string ticker)
        {
            CompanyDepth returnItem = new CompanyDepth();
            returnItem.mPriceComparison = new List<PriceComparison>();
            returnItem.mTickerName = ticker;

            var url = "https://www.cse.com.bd/market/market_depth/" + ticker;
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);

            var buyandsell = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("bodycol_3")).ToList();

            returnItem.mBuyOrders = buyandsell[0].InnerText.ToString().Split(delims, StringSplitOptions.RemoveEmptyEntries);
            returnItem.mSellOrders = buyandsell[1].InnerText.ToString().Split(delims, StringSplitOptions.RemoveEmptyEntries);

            var pricecomparison = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("bodycol_6")).ToList();
            var Price_Comparison = pricecomparison[0].InnerText.ToString().Split(delims, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < Price_Comparison.Count(); i+= 17)
            {
                if (Price_Comparison.Count() <= 19)
                {
                    PriceComparison PCObjempty = new PriceComparison();
                    PCObjempty.TickerName = ticker;
                    PCObjempty.State = "0";
                    PCObjempty.Opening_Price = "0";
                    PCObjempty.High_Price = "0";
                    PCObjempty.Low_Price = "0";
                    PCObjempty.LastTrade_Price = "0";
                    PCObjempty.NumberOfTrades = "0";
                    PCObjempty.Total_Volume = "0";
                    PCObjempty.Closing_Price = "0";
                    PCObjempty.Total_Value = "0";
                    returnItem.mPriceComparison.Add(PCObjempty);
                    continue;
                }
                PriceComparison listObj = new PriceComparison();
                listObj.State = Price_Comparison[i];
                listObj.Opening_Price = Price_Comparison[i + 2];
                listObj.High_Price = Price_Comparison[i + 4];
                listObj.Low_Price = Price_Comparison[i + 6];
                listObj.LastTrade_Price = Price_Comparison[i + 8];
                listObj.NumberOfTrades = Price_Comparison[i + 10];
                listObj.Total_Volume = Price_Comparison[i + 12];
                listObj.Closing_Price = Price_Comparison[i + 14];
                listObj.Total_Value = Price_Comparison[i + 16];
                returnItem.mPriceComparison.Add(listObj);
            }

            var fundamentalData = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("bodycol_12")).ToList();
            returnItem.mFundamentalData = fundamentalData[4].InnerText.ToString().Split(delims, StringSplitOptions.RemoveEmptyEntries);

            var newsfull = fundamentalData[5].InnerText.ToString().Trim().Split(delims, StringSplitOptions.RemoveEmptyEntries);
            var dateandtitle = newsfull[1].Split(':');
            var tickerandcontent = newsfull[2].Split(':');
            var newsobj = new News(dateandtitle[0], dateandtitle[1], tickerandcontent[0], tickerandcontent[1]);
            returnItem.mCompanyNews = newsobj;

            return returnItem;
        }

        /// <summary>
        /// WILL BE USED IN DAY START AND EOD
        /// This fucntion returns News objects list from Todays News in CSE
        /// </summary>
        /// <param name="mNewsList"></param>
        private static List<News> GetTodaysMarketNews()
        {
            List<News> returnList = new List<News>();
            var url = "https://www.cse.com.bd/media/news";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);
            //contains entire source code for the display of news
            var temp = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("news_inner_body")).ToList();
            //list of individual news 
            var news = temp[0].Descendants("div").ToList();
            foreach (var item in news)
            {
                var tempfull = item.InnerText.ToString().Trim();
                int startcolon = tempfull.IndexOf(':');
                var date = tempfull.Substring(0, startcolon);
                int starttab = tempfull.IndexOf("\t");
                var title = tempfull.Substring(startcolon+2, starttab).Trim();
                var secondtemp = tempfull.Substring(starttab+1).Trim();
                int secondcolon = secondtemp.IndexOf(':');
                var classification = secondtemp.Substring(0,secondcolon);
                var content = secondtemp.Substring(secondcolon + 2);
                var newsobj = new News(date, title, classification, content);

                returnList.Add(newsobj);
            }
            return returnList;
        }

        /// <summary>
        /// WILL BE USED DURING EOD.
        /// This fucntion adds more information to the objects in the parameter. 
        /// List of Company Objects must be provided, more data will be added to each object.
        /// </summary>
        /// <param name="mlistofCompanies"></param>
        private static  void GetCompanyInfo(List<Company> mlistofCompanies)
        {
            char[] delims = new[] { '\r', '\n', '\t' };
            for (int i = 0; i < 101; i++)
            {
                var url = "https://www.cse.com.bd/company/companydetails/" + mlistofCompanies[i].Tickername;

                var httpClient = new HttpClient();
                var html = httpClient.GetStringAsync(url);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html.Result);

                var tab = htmlDocument.DocumentNode.Descendants("div")
                            .Where(node => node.GetAttributeValue("class", "")
                            .Equals("com_inner_body")).ToList();

                var tables = tab[0].Descendants("table").ToList();

                mlistofCompanies[i].mArrayBasicInfo = tables[8].InnerText.ToString().Replace("&nbsp;", "").Replace(" ", "")
                                    .Split(delims, StringSplitOptions.RemoveEmptyEntries);

                mlistofCompanies[i].mArrayOtherInfo = tables[12].InnerText.ToString().Replace("&nbsp;", "").Replace(" ", "")
                                    .Split(delims, StringSplitOptions.RemoveEmptyEntries);

                mlistofCompanies[i].mArrayShareHoldingPercentage = tables[17].InnerText.ToString().Replace("&nbsp;", "").Replace(" ", "")
                                    .Split(delims, StringSplitOptions.RemoveEmptyEntries);

                mlistofCompanies[i].mArrayFinancialPerformance = tables[21].InnerText.ToString().Replace("&nbsp;", "").Replace(" ", "")
                                    .Split(delims, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        /// <summary>
        /// WILL BE USED BASED ON SEARCH
        /// This fucntion is used to get the depth information such as News, Buy orders,
        /// Sell Orders, Price Comparison and fundamental data of every instruments
        /// provided in the list of companies.
        /// </summary>
        /// <param name="mlistofCompanies"></param>
        /// <param name="mDepthList"></param>
        private static void GetIndividualDepth(List<Company> mlistofCompanies, List<CompanyDepth> mDepthList)
        {
            char[] delims = new[] { '\r', '\n','\t' };
            foreach (var item in mlistofCompanies)
            {
                CompanyDepth listObj = new CompanyDepth();
                listObj.mTickerName = item.Tickername;
                listObj.mPriceComparison = new List<PriceComparison>();
                var url = "https://www.cse.com.bd/market/market_depth/" + item.Tickername;
                var httpClient = new HttpClient();
                var html = httpClient.GetStringAsync(url);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html.Result);

                var buyandsell = htmlDocument.DocumentNode.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("bodycol_3")).ToList();

                listObj.mBuyOrders = buyandsell[0].InnerText.ToString().Split(delims, StringSplitOptions.RemoveEmptyEntries);
                listObj.mSellOrders = buyandsell[1].InnerText.ToString().Split(delims, StringSplitOptions.RemoveEmptyEntries);

                var pricecomparison = htmlDocument.DocumentNode.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("bodycol_6")).ToList();
                string[] stringsPC = pricecomparison[0].InnerText.ToString().Split(delims, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 1; i < stringsPC.Count(); i += 17)
                {
                    if(stringsPC.Count() <= 19)
                    {
                        PriceComparison PCObjempty = new PriceComparison();
                        PCObjempty.TickerName = item.Tickername;
                        PCObjempty.State = "0";
                        PCObjempty.Opening_Price = "0";
                        PCObjempty.High_Price = "0";
                        PCObjempty.Low_Price = "0";
                        PCObjempty.LastTrade_Price = "0";
                        PCObjempty.NumberOfTrades = "0";
                        PCObjempty.Total_Volume = "0";
                        PCObjempty.Closing_Price = "0";
                        PCObjempty.Total_Value = "0";
                        listObj.mPriceComparison.Add(PCObjempty);
                        continue;
                    }
                    PriceComparison PCObj = new PriceComparison();
                    PCObj.TickerName = item.Tickername;
                    PCObj.State = stringsPC[i];
                    PCObj.Opening_Price = stringsPC[i + 2];
                    PCObj.High_Price = stringsPC[i + 4];
                    PCObj.Low_Price = stringsPC[i + 6];
                    PCObj.LastTrade_Price = stringsPC[i + 8];
                    PCObj.NumberOfTrades = stringsPC[i + 10];
                    PCObj.Total_Volume = stringsPC[i + 12];
                    PCObj.Closing_Price = stringsPC[i + 14];
                    PCObj.Total_Value = stringsPC[i + 16];
                    listObj.mPriceComparison.Add(PCObj);
                }

                var fundamentalData = htmlDocument.DocumentNode.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("bodycol_12")).ToList();
                listObj.mFundamentalData = fundamentalData[4].InnerText.ToString().Split(delims, StringSplitOptions.RemoveEmptyEntries);

                var newsfull = fundamentalData[5].InnerText.ToString().Trim().Split(delims, StringSplitOptions.RemoveEmptyEntries);
                var dateandtitle = newsfull[1].Split(':');
                var tickerandcontent = newsfull[2];
                var newsobj = new News(dateandtitle[0], dateandtitle[1], item.Tickername , tickerandcontent);
                /////
                listObj.mCompanyNews = newsobj;
                mDepthList.Add(listObj);
            }
       }

        /// <summary>
        /// WILL BE USED DURING THE START OF THE DAY
        /// This fucntion return list of Circuit breaker objects
        /// </summary>
        /// <param name="mlistofCircuitBreaker"></param>
        private static List<CBItem> GetListofCircuitBreaker()
        {
            List<CBItem> returnList = new List<CBItem>();
            string url = "https://www.cse.com.bd/market/circuit_breaker";
            HttpClient httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);
            // has entrie source html in cblist[0].innerhtml
            var cblist = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("records_content")).ToList();

            var cblist2 = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("circuitbreaker_tabs_cont")).ToList();

            foreach (var item in cblist2)
            {
                char[] delims = new[] { ' ','\r', '\n' };
                string all = item.InnerText.ToString().Trim().Replace("\t", "");
                string[] strings = all.Split(delims, StringSplitOptions.RemoveEmptyEntries);
                CBItem listObj = new CBItem(strings[0], strings[1], strings[2], strings[3], strings[4], strings[5]);
                returnList.Add(listObj);
            }
            return returnList;
        }

        /// <summary>
        /// WILL BE USED BEFORE EOD.
        /// This fucntion returns list of companies by category.
        /// </summary>
        /// <param name="mlistofCompanies"></param>
        private static void GetListOfCompanies(List<Company> mlistofCompanies)
        {

            var url = "https://www.cse.com.bd/company/listedcompanies";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);

            var CompanyClass = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("id", "")
                .Equals("top_content_3")).ToList();

            var CompanyByCategory = CompanyClass[0].Descendants("ul").ToList();

            var companyAList = CompanyByCategory[0].Descendants("li").ToList();

            var companyBList = CompanyByCategory[1].Descendants("li").ToList();

            var companyNList = CompanyByCategory[2].Descendants("li").ToList();

            var companyZList = CompanyByCategory[3].Descendants("li").ToList();

            foreach (var item in companyAList)
            {
                var str = (item.InnerText.ToString().Trim().Replace("\t", "").Replace("\n", ""));

                int lastpositionofclosing = str.LastIndexOf(")");
                int lastpositionofopening = str.LastIndexOf("(");
                int len = lastpositionofclosing - lastpositionofopening;
                int test = str.LastIndexOf("(", lastpositionofopening - 1);

                if (test > -1 && lastpositionofclosing - test <= 12  && test != lastpositionofopening)
                {
                    lastpositionofopening = test;
                    len = lastpositionofclosing - lastpositionofopening;
                }
                string tickerstr = str.Substring(lastpositionofopening+1, len-1);
                var listitem = new Company { Category = "A", CompanyName = str, Tickername = tickerstr };
                mlistofCompanies.Add(listitem);
            }
            foreach (var item in companyBList)
            {
                var str = (item.InnerText.ToString().Trim().Replace("\t", "").Replace("\n", ""));
                int lastpositionofclosing = str.LastIndexOf(")");
                int lastpositionofopening = str.LastIndexOf("(");
                int len = lastpositionofclosing - lastpositionofopening;
                int test = str.LastIndexOf("(", lastpositionofopening - 1);
                if (test > -1 && lastpositionofclosing - test <= 12 && test != lastpositionofopening)
                {
                    lastpositionofopening = test;
                    len = lastpositionofclosing - lastpositionofopening;
                }
                string tickerstr = str.Substring(lastpositionofopening + 1, len - 1);
                var listitem = new Company { Category = "B", CompanyName = str, Tickername = tickerstr };
                mlistofCompanies.Add(listitem);
            }
            foreach (var item in companyNList)
            {
                var str = (item.InnerText.ToString().Trim().Replace("\t", "").Replace("\n", ""));
                int lastpositionofclosing = str.LastIndexOf(")");
                int lastpositionofopening = str.LastIndexOf("(");
                int len = lastpositionofclosing - lastpositionofopening;
                int test = str.LastIndexOf("(", lastpositionofopening - 1);
                if (test > -1 && lastpositionofclosing - test <= 12 && test != lastpositionofopening)
                {
                    lastpositionofopening = test;
                    len = lastpositionofclosing - lastpositionofopening;
                }
                string tickerstr = str.Substring(lastpositionofopening + 1, len - 1);
                var listitem = new Company { Category = "N", CompanyName = str, Tickername = tickerstr };
                mlistofCompanies.Add(listitem);
            }
            foreach (var item in companyZList)
            {
                var str = (item.InnerText.ToString().Trim().Replace("\t", "").Replace("\n", ""));
                int lastpositionofclosing = str.LastIndexOf(")");
                int lastpositionofopening = str.LastIndexOf("(");
                int len = lastpositionofclosing - lastpositionofopening;
                int test = str.LastIndexOf("(", lastpositionofopening - 1);
                if (test > -1 && lastpositionofclosing - test <= 12 && test != lastpositionofopening)
                {
                    lastpositionofopening = test;
                    len = lastpositionofclosing - lastpositionofopening;
                }
                string tickerstr = str.Substring(lastpositionofopening + 1, len - 1);
                var listitem = new Company { Category = "Z", CompanyName = str, Tickername = tickerstr };
                mlistofCompanies.Add(listitem);
            }
        }
    }
}
