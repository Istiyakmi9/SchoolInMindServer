namespace BottomhalfCore.FactoryContext
{
    public class WeavProxy<T>
    {
        //private readonly T _decorated;
        //private AopDetail ObjAopDetail;
        //private readonly IContainer container;
        //private readonly BeanContext context;
        //private Object Bean;

        //public WeavProxy(T decorated) : base(typeof(T))
        //{
        //    this._decorated = decorated;
        //    container = Container.GetInstance();
        //    context = BeanContext.GetInstance();
        //}

        //public override IMessage Invoke(IMessage msg)
        //{
        //    MethodParameters ObjMethodParameters = null;
        //    var methodCall = msg as IMethodCallMessage;
        //    var methodInfo = methodCall.MethodBase as MethodInfo;
        //    string ClassName = (methodCall.TypeName.Split(new string[] { "[[" }, StringSplitOptions.RemoveEmptyEntries)[0]);
        //    var GenTypeList = container.GetImplementedType(ClassName);
        //    if (GenTypeList != null && GenTypeList.Count() > 0)
        //    {
        //        try
        //        {
        //            Object result = null;
        //            TypeRefCollection ClassRefInfo = null;
        //            ObjMethodParameters = new MethodParameters();
        //            ObjMethodParameters.MethodName = methodInfo.Name;
        //            ClassRefInfo = container.GetTypeRefByName(GenTypeList.FirstOrDefault().Name);
        //            ObjMethodParameters.Arguments = methodCall.InArgs;
        //            ObjAopDetail = ClassRefInfo.AppliedAopDetail;
        //            if (ObjAopDetail.MethodExpression == methodCall.MethodName)
        //            {
        //                Bean = context.GetBean(ObjAopDetail.AspectClassName);
        //                if (ObjAopDetail.AOPType == "BeforeAdvice" || ObjAopDetail.AOPType == "AroundAdvice")
        //                {
        //                    MethodInfo Method = Bean.GetType().GetMethod("MethodInvoke", new Type[] { typeof(MethodParameters) });
        //                    if (Method != null)
        //                        result = Method.Invoke(Bean, new object[] { ObjMethodParameters });
        //                }

        //                if (result != null && Convert.ToBoolean(result))
        //                {
        //                    result = methodInfo.Invoke(_decorated, methodCall.InArgs);
        //                    if (ObjAopDetail.AOPType == "AfterAdvice" || ObjAopDetail.AOPType == "AroundAdvice")
        //                    {
        //                        MethodInfo Method = Bean.GetType().GetMethod("MethodInvoke", new Type[] { typeof(MethodParameters), typeof(Object) });
        //                        if (Method != null)
        //                            Method.Invoke(Bean, new object[] { ObjMethodParameters, result });
        //                    }
        //                }
        //            }
        //            return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
        //        }
        //        catch (Exception e)
        //        {
        //            Bean = context.GetBean(ObjAopDetail.AspectClassName);
        //            if (ObjAopDetail.AOPType == "ThrowAdvice" || ObjAopDetail.AOPType == "AroundAdvice")
        //            {
        //                MethodInfo Method = Bean.GetType().GetMethod("MethodInvoke");
        //                if (Method != null)
        //                    Method.Invoke(Bean, null);
        //            }
        //            return new ReturnMessage(e, methodCall);
        //        }
        //    }
        //    return new ReturnMessage(null, methodCall);
        //}
    }
}
