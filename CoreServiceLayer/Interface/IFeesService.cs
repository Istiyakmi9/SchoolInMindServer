using CommonModal.Models;

namespace ServiceLayer.Interface
{
    public interface IFeesService
    {
        string FetchFeesService(SearchModal searchModal);

        string ManageFeesService(Holidays holidays);

        string DeleteFeesService(Holidays holidays);
    }
}
