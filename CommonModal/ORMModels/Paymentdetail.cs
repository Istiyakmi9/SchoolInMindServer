using System;

namespace CommonModal.ORMModels
{
	public class Paymentdetail
	{
		public string PaymentDetailId { set; get; }
		public string SchooltenentId { set; get; }
		public string UserId { set; get; }
		public int? PaymentForMonth { set; get; }
		public int? ForYear { set; get; }
		public string PaymentVendorId { set; get; }
		public string PaymentId { set; get; }
		public string Status { set; get; }
		public string UnMappedStatus { set; get; }
		public DateTime AddedOn { set; get; }
		public string Provider { set; get; }
		public string PaymentSalt { set; get; }
		public string PaymentKey { set; get; }
		public string ServiceProvider { set; get; }
	}
}
