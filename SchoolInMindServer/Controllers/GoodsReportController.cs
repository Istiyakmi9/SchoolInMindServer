using BottomhalfCore.FactoryContext;
using BottomhalfCore.Annotations;
using CommonModal.Models;
using SchoolInMindServer.Modal;
using Microsoft.AspNetCore.Mvc;
using CoreServiceLayer.Implementation;
using ServiceLayer.Interface;

namespace SchoolInMindServer.Controllers
{
    [Transient]
    public class GoodsReportController : ControllerBase
    {
        // GET: GoodsReport
        private readonly IGoodsItemService<GoodsItemService> objGoodsItemService;
        private readonly IVendorService<VendorService> ObjVendorService;
        private readonly BeanContext context;
        public GoodsReportController(GoodsItemService objGoodsItemService, VendorService ObjVendorService, Vendor ObjGoods)
        {
            this.ObjVendorService = ObjVendorService;
            this.objGoodsItemService = objGoodsItemService;
            this.context = BeanContext.GetInstance();
        }

        public IResponse<ApiResponse> AvailableGoods(string SearchStr, string SortBy, string PageIndex, string PageSize)
        {
            ///ViewBag.Result = GoodsItemFilter(null, null, null, null);
            return null;
        }

        public IResponse<ApiResponse> AddEditGoodsItem(string GoodsUid)
        {
            //ViewBag.Result = objGoodsItemService.GoodsItemByUidService(GoodsUid);
            return null;
        }

        public IResponse<ApiResponse> Vender(string SearchStr, string SortBy, string PageIndex, string PageSize)
        {
            //ViewBag.Result = Items(null, null, null, null);
            return null;
        }

        public IResponse<ApiResponse> AddVendor(string vendorId)
        {
            //ViewBag.Result = VendorByUid(vendorId);
            return null;
        }

        private IResponse<ApiResponse> VendorByUid(string vendorUid)
        {
            Vendor ObjVendor = ObjVendorService.GetVendorByUid(vendorUid);
            return null;
        }

        public IResponse<ApiResponse> AddClient(string ClientUid, string tenentId)
        {
            //if (!string.IsNullOrEmpty(ClientUid) && !string.IsNullOrEmpty(tenentId))
            //    ViewBag.Result = objGoodsItemService.GetClientByUidService(ClientUid, tenentId);
            //else
            //    ViewBag.Result = null;
            return null;
        }

        [HttpPost]
        public IResponse<ApiResponse> AddClientToDb(Clients ObjClient)
        {
            ServiceResult ObjServiceResult = objGoodsItemService.AddClientService(ObjClient);
            return null;
        }

        [HttpPost]
        public IResponse<ApiResponse> AddVendorToDb(Vendor ObjVendor)
        {
            ServiceResult ObjServiceResult = ObjVendorService.RegisterVendorService(ObjVendor);
            return null;
        }

        public IResponse<ApiResponse> AddGoods()
        {
            //ViewBag.Vendors = objGoodsItemService.GetVendorDetail();
            return null;
        }

        [HttpPost]
        public IResponse<ApiResponse> AddGoodsItem(Vendor ObjGoods)
        {
            string ResultedData = null;
            ResultedData = objGoodsItemService.AddNewGoodsItem(ObjGoods);
            return null;
        }

        [HttpPost]
        public IResponse<ApiResponse> AddClient(Clients ObjClient)
        {
            ServiceResult result = objGoodsItemService.AddClientService(ObjClient);
            return null;
        }

        public IResponse<ApiResponse> Client(string SearchStr, string SortBy, string PageIndex, string PageSize)
        {
            //ViewBag.Result = objGoodsItemService.ClientFilterService(null, null, null, null);
            return null;
        }

        [HttpGet]
        public IResponse<ApiResponse> Items(string SearchString, string SortBy, string PageIndex, string PageSize)
        {
            string Result = objGoodsItemService.ItemList(SearchString, SortBy, PageIndex, PageSize);
            if (Result == null)
                Result = "";
            return null;
        }

        [HttpGet]
        public IResponse<ApiResponse> GoodsItemFilter(string SearchString, string SortBy, string PageIndex, string PageSize)
        {
            string Result = objGoodsItemService.GoodsItemFilterService(SearchString, SortBy, PageIndex, PageSize);
            if (Result == null)
                Result = "";
            return null;
        }

        [HttpGet]
        public IResponse<ApiResponse> GetItemPreFetchDetail()
        {
            string Result = objGoodsItemService.GetItemPreFetchDetailService();
            if (Result == null)
                Result = "";
            return null;
        }

        [HttpGet]
        public IResponse<ApiResponse> ClientFilter(string SearchString, string SortBy, string PageIndex, string PageSize)
        {
            string Result = objGoodsItemService.ClientFilterService(SearchString, SortBy, PageIndex, PageSize);
            if (Result == null)
                Result = "";
            return null;
        }

        [HttpPost]
        public IResponse<ApiResponse> UpdateSingleGoods(GoodsItem ObjGoodsItem)
        {
            ServiceResult Result = objGoodsItemService.UpdateSingleGoodService(ObjGoodsItem);
            return null;
        }
    }
}