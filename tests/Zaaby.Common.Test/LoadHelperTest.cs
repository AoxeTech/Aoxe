using System;
using Xunit;

namespace Zaaby.Common.Test
{
    public class UnitTest1
    {
        [Fact]
        public void GetByAttribute()
        {
            var types0 = LoadHelper.GetByAttribute<TestAttribute>();
            var types1 = LoadHelper.GetByAttribute(typeof(TestDerivedAttribute));
        }
    }

    public class TestAttribute : Attribute
    {
    }

    public class TestDerivedAttribute : TestAttribute
    {
    }

    [TestAttribute]
    public class TestClass
    {
    }

    [TestDerivedAttribute]
    public class TestDerivedClass
    {
    }
}