using System;

namespace CommonModal.ORMModels
{
	public class Paymentvendors
	{
		public string PaymentVendorId { set; get; }
		public string SchooltenentId { set; get; }
		public string VendorName { set; get; }
		public string Salt { set; get; }
		public string Key { set; get; }
		public string Provider { set; get; }
		public DateTime CreatedOn { set; get; }
		public DateTime? UpdatedOn { set; get; }
		public string CreatedBy { set; get; }
		public string UpdatedBy { set; get; }
	}
}
