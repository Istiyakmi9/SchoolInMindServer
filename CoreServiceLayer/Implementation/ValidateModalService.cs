using BottomhalfCore.FactoryContext;
using BottomhalfCore.Annotations;
using CommonModal.Models;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CoreServiceLayer.Implementation
{
    [Transient]
    public class ValidateModalService : IValidateModalService<ValidateModalService>
    {
        private readonly BeanContext context = null;
        private readonly ServiceResult serviceResult = null;
        public ValidateModalService(ServiceResult serviceResult)
        {
            context = BeanContext.GetInstance();
            this.serviceResult = serviceResult;
        }

        public void ValidateSeachModal(SearchModal searchModal)
        {
            if (string.IsNullOrEmpty(searchModal.SearchString))
                searchModal.SearchString = "1=1";
            if (string.IsNullOrEmpty(searchModal.SortBy))
                searchModal.SortBy = "Class";
            if (searchModal.PageIndex <= 0)
                searchModal.PageIndex = 1;
            if (searchModal.PageSize <= 0)
                searchModal.PageSize = 15;
        }

        public ServiceResult ValidateModalFieldsService<T>(dynamic ReferencedObject)
        {

            Type ObjectType = typeof(T);
            ServiceResult serviceResult = null;
            IList<string> ErrorColumnName = new List<string>();
            PropertyInfo[] fields = ObjectType.GetProperties();
            CustomAttributeData Attr = null;
            int Index = 0;
            while(Index < fields.Length)
            {
                int InnerIndex = 0;
                while (InnerIndex < fields[Index].CustomAttributes.Count())
                {
                    Attr = fields[Index].CustomAttributes.ElementAt(InnerIndex);
                    if (Attr.AttributeType.Name == "Required")
                    {
                        if (fields[Index].PropertyType == typeof(System.String))
                        {
                            var Data = fields[Index].GetValue(ReferencedObject);
                            if (Data == null || Data == "")
                                ErrorColumnName.Add(fields[Index].Name);
                        }
                    }
                    InnerIndex++;
                }
                Index++;
            }

            serviceResult = context.GetBean<ServiceResult>();
            if (ErrorColumnName.Count > 0)
                serviceResult.IsValidModal = false;
            else
                serviceResult.IsValidModal = true;
            serviceResult.ErrorResultedList = ErrorColumnName;
            return serviceResult;
        }

        public ServiceResult ValidateModalFieldsService(Type ObjectName, dynamic ReferencedObject)
        {
            Type ObjectType = ObjectName;
            ServiceResult serviceResult = null;
            IList<string> ErrorColumnName = new List<string>();
            PropertyInfo[] fields = ObjectType.GetProperties();
            foreach (var ObjectField in fields)
            {
                foreach (var Attr in ObjectField.CustomAttributes)
                {
                    if (Attr.AttributeType.Name == "Required")
                    {
                        if (ObjectField.PropertyType.Name.ToLower() == "string")
                        {
                            var Data = ObjectField.GetValue(ReferencedObject);
                            if (Data == null || Data == "")
                                ErrorColumnName.Add(ObjectField.Name);
                        }
                    }
                }
            }

            serviceResult = context.GetBean<ServiceResult>();
            if (ErrorColumnName.Count > 0)
                serviceResult.IsValidModal = false;
            else
                serviceResult.IsValidModal = true;
            serviceResult.ErrorResultedList = ErrorColumnName;
            return serviceResult;
        }
    }
}
