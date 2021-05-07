using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModal.Models
{
    public class StaffRegistrationForm : RegistrationFormData
    {
        public string VehicleTypeId { set; get; }
        public string VehicleNumber { set; get; }
        public string VehicleRegistrationNo { set; get; }
        public string LicenceNumber { set; get; }
        public string VehicleDetailId { set; get; }
    }
}
