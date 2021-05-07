using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModal.Models
{
    public class Vendor
    {
        public IList<string> ObjData;
        public Vendor()
        {
            this.ObjData = new List<string>();
        }
        [Required]
        public string AdminId { set; get; }
        public string VendorUId { set; get; }
        [Required]
        public string TenentId { set; get; }
        public string SellerFirstName { set; get; }
        public string SellerLastName { set; get; }
        public string ShopName { set; get; }
        public string FullAddress { set; get; }
        public string Mobile { set; get; }
        public string Email { set; get; }
        public string GSTIN { set; get; }
        public string BankName { set; get; }
        public string AccountNo { set; get; }
        public Double PurchasedAmount { set; get; } = 0;
        public DateTime PurchasedOn { set; get; }
        [Required]
        public string IFSCCode { set; get; }
        public string ExistingVendorUid { set; get; }
        public string GoodsAsXml { set; get; }
        public string InvoiceNo { set; get; }
    }
}
