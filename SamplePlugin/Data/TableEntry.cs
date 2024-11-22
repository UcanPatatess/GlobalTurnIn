using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplePlugin.Data
{
    public class TableEntry
    {
        public int Shop { get; set; }
        public int ItemID { get; set; }
        public int BuyAmount { get; set; }
        public int BuyItemID { get; set; }
        public int Slot { get; set; }
        public int ShopType { get; set; }
    }
}
