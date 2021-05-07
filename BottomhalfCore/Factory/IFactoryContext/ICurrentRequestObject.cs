namespace BottomhalfCore.IFactoryContext
{
    public interface ICurrentRequestObject<T>
    {
        string SessionKey { set; get; }
        //RequestContext CurrentRequestContext { set; get; }
    }
}
