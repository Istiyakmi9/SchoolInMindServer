using CommonModal.Models;

namespace ServiceLayer.Interface
{
    public interface INoticeService
    {
        string FetchNoticeService(SearchModal searchModal);

        string ManageNoticeService(Holidays holidays);

        string DeleteNoticeService(Holidays holidays);
    }
}
