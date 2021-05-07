using System;
using System.Collections.Generic;

namespace CommonModal.Models
{
    public class FeesPaymentDetail
    {
        public IList<FeesDetail> feesDetail { set; get; }
        public IList<string> Detail { set; get; }
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
        public string VendorBaseUrl { set; get; }
        public string ServiceProvider { set; get; }
        public string SuccessUrl { set; get; }
        public string FailUrl { set; get; }
        public string AdminId { set; get; }
        public double TotalAmount { set; get; }
        public int TxnIdExists { set; get; }
        public string TxnUid { set; get; }
    }

    public class FeesDetail
    {
        public int ForMonth { set; get; }
        public int ForYear { set; get; }
        public int FeeCode { set; get; }
        public int FineCode { set; get; }
        public string MonthName { set; get; }
        public double FineAmount { set; get; }
        public double Amount { set; get; }
        public string StudentUid { set; get; }
    }
}