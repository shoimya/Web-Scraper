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
    public class CBItem
    {
        public string SL { get; set; }
        public string Scrip_Code { get; set; }
        public string Floor_Price { get; set; }
        public string Min_Allowed_Price { get; set; }
        public string Closed_Price { get; set; }
        public string Max_Allowed_price { get; set; }

        public CBItem(string sl, string scode, string floorprice, string minprice, string closedprice, string maxprice)
        {
            this.SL = sl;
            this.Scrip_Code = scode;
            this.Floor_Price = floorprice;
            this.Min_Allowed_Price = minprice;
            this.Closed_Price = closedprice;
            this.Max_Allowed_price = maxprice;
        }

        public override string ToString() => $"{SL} {Scrip_Code} {Min_Allowed_Price} {Closed_Price} {Max_Allowed_price}";
    }
}
