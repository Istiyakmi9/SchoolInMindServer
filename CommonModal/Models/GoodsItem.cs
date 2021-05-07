using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModal.Models
{
    public class GoodsItem
    {
        public string GoodsItemUid { set; get; }
        public string TenentId { set; get; }
        [Required]
        public string GoodsUid { set; get; }
        [Required]
        public string Item { set; get; }
        [Required]
        public string ItemName { set; get; }
        public string ItemCompanyName { set; get; }
        [Required]
        public string HSNCode { set; get; }
        public float salePrice { set; get; }
        public float PurchasedPrice { set; get; }
        public float MRP { set; get; }
        public int Unit { set; get; }
        public float M_Qty { set; get; }
        public float Discount { set; get; }
        [Required]
        public string GST { set; get; }
        public string BatchNo { set; get; }
        public string CreatedOn { set; get; }
        public string UpdatedOn { set; get; }
    }
}
