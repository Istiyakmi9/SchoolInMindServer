using CommonModal.ORMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModal.Models
{
    public class SoldItems
    {
        public IList<SoldItemDetail> objItemDetail { set; get; }
        public Clients ObjClientDetail { set; get; }
        public string ClientUid { set; get; }
        public string AdharNumber { set; get; }
        public string PersonName { set; get; }
        public string ShopName { set; get; }
        public string BankName { set; get; }
        public string AccountNo { set; get; }
        public string IFSCCode { set; get; }
        public string GSTIN { set; get; }
        public string FullAddress { set; get; }
        public string Mobile { set; get; }
        public string Email { set; get; }
        public string GoodsAsXml { set; get; }
        public string ExistingClientUid { set; get; }
        public string FileName { set; get; }
        public DateTime? OrderedDate { set; get; }
        public int NewShippingAddress { set; get; }
        public string AddressUniqueCode { set; get; }
        public string PaymentMode { set; get; }
        public string GuestUserName { set; get; }
        public string PaymentType { set; get; }
        public int PayeeCode { set; get; }
        public string Latitude { set; get; }
        public string Longitude { set; get; }
        public string Mihpayid { set; get; }
        public string BankStatus { set; get; }
        public string UnmappedStatus { set; get; }
        public string OrderStatus { set; get; }
        public string VerificationCode { set; get; }
        public DateTime? DeliveryDateTime { set; get; }
        public string MarchandKey { set; get; }
        public string Salt { set; get; }
        public string PayUBaseUrl { set; get; }
        public string ServiceProvider { set; get; }
        public string PayUSuccessUrl { set; get; }
        public string PayUFailUrl { set; get; }
        public string AdminId { set; get; }
        public string TxnUid { set; get; }
        public int TxnIdExists { set; get; }
        public double TotalAmount { set; get; }
    }

    public class SoldItemDetail
    {
        public string ItemCode { set; get; }
        public string Item { set; get; }
        public string Company { set; get; }
        public string HSNCode { set; get; }
        public string ItemName { set; get; }
        public double SellingPrice { set; get; }
        public double MRP { set; get; }
        public double Unit { set; get; }
        public double TotalAmount { set; get; }
        public double MQty { set; get; }
        public int ItemCount { set; get; }
        public int ItemDicount { set; get; }
        public double NewPrice { set; get; }
        public int NewDiscount { set; get; }
        public string Description { set; get; }
    }
}
