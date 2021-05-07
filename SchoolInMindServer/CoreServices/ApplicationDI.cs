using BottomhalfCore.FactoryContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;

namespace Education.CoreServices
{
    public class ApplicationDI : IControllerFactory
    {
        public object CreateController(ControllerContext context)
        {
            try
            {
                BeanContext beanContext = BeanContext.GetInstance();
                Type ClassType = context.ActionDescriptor.ControllerTypeInfo.AsType();
                Object NewInstance = beanContext.GetBean(ClassType, context.HttpContext);
                return NewInstance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ReleaseController(ControllerContext context, object controller)
        {
            //throw new NotImplementedException();
        }
    }
}
