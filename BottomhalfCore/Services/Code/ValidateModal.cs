using BottomhalfCore.BottomhalfModel;
using BottomhalfCore.FactoryContext;
using BottomhalfCore.Services.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BottomhalfCore.Services.Code
{
    public class ValidateModal : IValidateModal<ValidateModal>
    {
        private readonly BeanContext context = null;
        private readonly ServiceResult ObjServiceResult = null;
        public ValidateModal()
        {
            context = BeanContext.GetInstance();
            this.ObjServiceResult = new ServiceResult();
        }
        public ServiceResult ValidateModalFieldsService(Type ObjectName, dynamic ReferencedObject)
        {
            Type ObjectType = ObjectName;
            ServiceResult ObjServiceResult = null;
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

            ObjServiceResult = new ServiceResult();
            if (ErrorColumnName.Count > 0)
                ObjServiceResult.IsValidModal = false;
            else
                ObjServiceResult.IsValidModal = true;
            ObjServiceResult.ErrorResultedList = ErrorColumnName;
            return ObjServiceResult;
        }
    }
}
