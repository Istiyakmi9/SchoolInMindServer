using System;
using System.ComponentModel.DataAnnotations;

namespace CommonModal.Models
{
    public class Clients
    {
        public string ClientUid { set; get; }
        [Required]
        public string TenentId { set; get; }
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
        public DateTime CreatedOn { set; get; }
    }
}



