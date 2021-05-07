using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.Annotations
{
    /// <summary>
    /// <para />expression: returntype FullyQualifiedNameSpace(arguments)
    /// <para />e.g * Test.Aspect *(...)
    /// <para />*: any return type, Test.Aspect: Under Test.Aspect Namespace , *: Applied on all methods, (...): Having any type and no. of arguments
    /// </summary>
    public class @BeforeAdvice : Attribute
    {
        public string ReturnType;
        public string NameSpace;
        public string MethodNameExpression;
        public string Arguments;
        public BeforeAdvice(string ReturnType, string NameSpace, string MethodNameExpression, string Arguments)
        {
            this.ReturnType = ReturnType;
            this.NameSpace = NameSpace;
            this.MethodNameExpression = MethodNameExpression;
            this.Arguments = Arguments;
        }
    }
}
