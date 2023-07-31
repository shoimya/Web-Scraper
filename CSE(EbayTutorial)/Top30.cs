using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE_EbayTutorial_
{
    class Top30
    {
        public string mOrderNumber { get; set; }
        public string mTickerName { get; set; }
        public string mLTP { get; set; }
        public string mHigh { get; set; }
        public string mLow { get; set; }
        //obly for CSE
        public string mOpen { get; set; }
        //only for DSE
        public string mClose { get; set; }
        public string mYCP { get; set; }
        //Only for DSE
        public string mRateChange { get; set; }
        //for Both
        public string mTrade { get; set; }
        public string mTradeValue { get; set; }
        public string mTradeVolume { get; set; }

    }
}
