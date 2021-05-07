using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModal.Models
{
    public class Goods
    {
        public string Item { set; get; }
        public string ItemName { set; get; }
        public string Company { set; get; }
        public string HSNCode { set; get; }
        public double SellingPrice { set; get; }
        public double MRP { set; get; }
        public int Unit { set; get; }
        public float MQty { set; get; }
    }
}
