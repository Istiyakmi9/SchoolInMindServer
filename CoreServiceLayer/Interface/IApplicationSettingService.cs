using CommonModal.Models;
using CommonModal.ORMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IApplicationSettingService<T>
    {
        string CreateOrUpdateServices(List<StoreZone> storeZone);
        DataSet DeleteService(int storeId);
        DataSet GetZone();
        string CreateRoomService(int RoomsCount);
        string GetRoomService(SearchModal searchModal);
        string UpdateRoomDetailService(RoomDetail roomDetail);

    }
}
