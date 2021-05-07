using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModal.Models
{
    public class sendEmail
    {
        public sendEmail()
        {
            itemDetailList = new List<itemDetail>();
        }
        public string Name { set; get; }
        public string Address { set; get; }
        public string EmailId { set; get; }
        public string MobileNo { set; get; }
        public string TotalAmount { set; get; }
        public IList<itemDetail> itemDetailList = null;
    }

    public class itemDetail
    {
        public string _itemName { set; get; }
        public string _itemDetail { set; get; }
        public string _itemQuantity { set; get; }
        public string _itemPrice { set; get; }


    }
}
