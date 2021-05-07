using CommonModal.Models;

namespace ServiceLayer.Interface
{
    public interface IMarkService
    {
        string FetchMarksService(SearchModal searchModal);

        string ManageMarksService(Holidays holidays);

        string DeleteMarksService(Holidays holidays);
    }
}
