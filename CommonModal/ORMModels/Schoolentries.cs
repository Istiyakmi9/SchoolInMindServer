using System;

namespace CommonModal.ORMModels
{
	public class Schoolentries
	{
		public string TenentId { set; get; }
		public string Schoolfullname { set; get; }
		public string LicenseNo { set; get; }
		public string AffiliatedBy { set; get; }
		public string Fulladdress { set; get; }
		public string State { set; get; }
		public string City { set; get; }
		public System.Int64? Pincode { set; get; }
		public DateTime CreatedOn { set; get; }
		public DateTime? UpdatedOn { set; get; }
	}
}
