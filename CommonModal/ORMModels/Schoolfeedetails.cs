using System;

namespace CommonModal.ORMModels
{
    public class Schoolfeedetails
    {
        public string SchoolFeeDetailId { set; get; }
        public string SchooltenentId { set; get; }
        public string ClassDetailUid { set; get; }
        public string FineForPayeeUid { set; get; }
        public int FeeCode { set; get; }
        public double Amount { set; get; }
        public bool IsFeeChanged { set; get; }
        public double NewAmount { set; get; }
        public DateTime? AffectedDate { set; get; }
        public int? PaymentDate { set; get; }
        public string LastPaymentDate { set; get; }
        public DateTime CreatedOn { set; get; }
        public DateTime? UpdatedOn { set; get; }
        public string CreatedBy { set; get; }
        public string UpdatedBy { set; get; }
    }
}
