using CommonModal.Models;

namespace CoreServiceLayer.Interface
{
    public interface IAssignmentService
    {
        string GetAssignments(SearchModal searchModal);
    }
}
