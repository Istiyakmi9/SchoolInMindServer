using CommonModal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IVendorService<T>
    {
        ServiceResult RegisterVendorService(Vendor objVendor);
        Vendor GetVendorByUid(string vendorUId);
    }
}
