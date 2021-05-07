using CommonModal.Models;
using CommonModal.ORMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface ISellGoodService<T>
    {
        string GetClientInfoService();
        string InsertClientDetail(SoldItems ObjSoldItems);
        Clients GetClientDetailByUid(string ExistingClientUid);
        SoldItems SoldGoodsService(SoldItems ObjSoldItems);
    }

    public class ClientInfo
    {
        public string ClientUid { set; get; }
        public string ClientName { set; get; }
    }
}
