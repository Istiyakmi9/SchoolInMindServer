using CommonModal.Models;

namespace ServiceLayer.Interface
{
    public interface IHolidayService
    {
        string GetHolidayListService(SearchModal searchModal);
        string ManageHolidayService(Holidays holidays);
        string DeleteHolidayService(Holidays holidays);
    }
}
