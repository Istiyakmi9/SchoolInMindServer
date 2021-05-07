using CommonModal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IValidateModalService<T>
    {
        ServiceResult ValidateModalFieldsService<U>(dynamic ReferencedObject);
        ServiceResult ValidateModalFieldsService(Type ObjectName, dynamic ReferencedObject);
        void ValidateSeachModal(SearchModal searchModal);
    }
}
