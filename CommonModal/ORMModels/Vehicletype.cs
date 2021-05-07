using System;

namespace CommonModal.ORMModels
{
	public class Vehicletype
	{
		public string VehicleTypeId { set; get; }
		public string SchooltenentId { set; get; }
		public string VehicleName { set; get; }
		public int? Capacity { set; get; }
		public string Fueltype { set; get; }
		public string CreatedBy { set; get; }
		public string UpdatedBy { set; get; }
	}
}
