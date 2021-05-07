using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface ITrackingService<T>
    {
        string GetMapDetailService(string DeviceId);
        string GetVehicleService(string SearchStr, string SortBy, string PageIndex, string PageSize);
    }
}
