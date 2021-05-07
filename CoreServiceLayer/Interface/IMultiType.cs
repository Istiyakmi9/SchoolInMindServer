using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IMultiType<T, G, K>
    {
    }

    public class TestClassA
    {

    }

    public class TestClassB
    {

    }

    public interface ITestIB<T>
    {

    }

    public class TestB : ITestIB<TestB>
    {

    }
}
