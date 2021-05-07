using System;
using System.ComponentModel.DataAnnotations;

namespace CommonModal.Models
{
    public class CommonFeesModal
    {
        public string SchoolFeeDetailId { set; get; }
        [Required]
        public string schooltenentId { set; get; }
        [Required]
        public string Class { set; get; }
        public string FineForPayeeUid { set; get; }
        [Required]
        public int FeeCode { set; get; }
        [Required]
        public double Amount { set; get; }
        public bool IsFeeChanged { set; get; }
        public double NewAmount { set; get; }
        [Required]
        public DateTime AffectedDate { set; get; }
        public int PaymentDate { set; get; }
        public string LastPaymentDate { set; get; }
        public int LateFineAmount { set; get; }
        public string LateFineType { set; get; }
        public int LateFinePerDayAmount { set; get; }
        public int LateFinePerMonthAmount { set; get; }

    }
}
