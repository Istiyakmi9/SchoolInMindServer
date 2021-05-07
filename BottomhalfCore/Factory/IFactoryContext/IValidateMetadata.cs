using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BottomhalfCore.IFactoryContext
{
    public interface IValidateMetadata
    {
        bool XmlIsValid();
        bool AnnotatedClassIsValid(XElement metaElements);
        bool ResolveReference(string refId, string beanId);
        bool ResolveCirularConstructor(string refId, string beanId);
    }
}
