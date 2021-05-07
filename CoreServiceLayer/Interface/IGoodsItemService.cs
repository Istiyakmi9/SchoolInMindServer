using CommonModal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IGoodsItemService<T>
    {
        string ItemList(string searchStr, string sortBy, string pageno, string pagesize);
        string GetVendorDetail();
        string AddNewGoodsItem(Vendor ObjGoods);
        string GoodsItemFilterService(string SearchString, string SortBy, string PageIndex, string PageSize);
        ServiceResult AddClientService(Clients ObjClients);
        string GetClientByUidService(string ClientUid, string tenentId);
        string ClientFilterService(string SearchString, string SortBy, string PageIndex, string PageSize);
        string GoodsItemByUidService(string GoodsItemUid);
        ServiceResult UpdateSingleGoodService(GoodsItem ObjGoodsItem);
        string GetItemPreFetchDetailService();
    }
}
