using System;

namespace CommonModal.ORMModels
{
	public class Vehicledetail
	{
		public string VehicleDetailId { set; get; }
		public string SchooltenentId { set; get; }
		public string VehicleRegistrationNo { set; get; }
		public string VehicleNumber { set; get; }
		public string StaffMemberUid { set; get; }
		public string VehicleTypeId { set; get; }
		public string StudentUid { set; get; }
		public DateTime CreatedOn { set; get; }
		public DateTime? UpdatedOn { set; get; }
		public string CreatedBy { set; get; }
		public string UpdatedBy { set; get; }
	}
}
