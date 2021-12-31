using System;
using System.Diagnostics;
using AliceServices;
using BobServices;
using CarolServices;
using IAliceServices;
using IBobServices;
using ICarolServices;
using Xunit;

namespace Zaaby.Shared.Test
{
    public class UnitTest1
    {
        [Fact]
        public void GetByAttribute()
        {
            var types0 = LoadHelper.GetByAttributes(typeof(TestAttribute));
            var types1 = LoadHelper.GetByAttributes(typeof(TestDerivedAttribute));

            Assert.Equal(2, types0.Count);
            Assert.Single(types1);
            Assert.Contains(typeof(TestClassWithAttribute), types0);
            Assert.Contains(typeof(TestClassWithDerivedAttribute), types0);
            Assert.Contains(typeof(TestClassWithDerivedAttribute), types1);
        }

        [Fact]
        public void GetByBaseType()
        {
            var types0 = LoadHelper.GetByBaseTypes(typeof(ITestInterface));
            var types1 = LoadHelper.GetByBaseTypes(typeof(IDerivedTestInterface));

            Assert.Equal(2, types0.Count);
            Assert.Contains(typeof(IDerivedTestInterface), types0);
            Assert.Contains(typeof(ClassWithInterface), types0);
            Assert.Contains(typeof(ClassWithDerivedInterface), types0);
        }

        [Fact]
        public void ScanTypesTest()
        {
            var sw = Stopwatch.StartNew();

            LoadHelper.FromAssemblyOf<IAliceService>();
            LoadHelper.FromAssemblyOf<AliceService>();
            LoadHelper.FromAssemblyOf<IBobService>();
            LoadHelper.FromAssemblyOf<BobService>();
            LoadHelper.FromAssemblyOf<ICarolService>();
            LoadHelper.FromAssemblyOf<CarolService>();

            sw.Stop();
            var i0 = sw.ElapsedMilliseconds;

            LoadHelper.LoadMode = LoadTypesMode.LoadByAllDirectory;

            sw.Restart();

            LoadHelper.LoadTypes();

            sw.Stop();
            var i1 = sw.ElapsedMilliseconds;

            Assert.True(LoadHelper.LoadSpecifyTypes().Count > 0);
        }
    }

    public class TestAttribute : Attribute
    {
    }

    public class TestDerivedAttribute : TestAttribute
    {
    }

    [Test]
    public class TestClassWithAttribute
    {
    }

    [TestDerived]
    public class TestClassWithDerivedAttribute
    {
    }

    public interface ITestInterface
    {
    }

    public interface IDerivedTestInterface : ITestInterface
    {
    }

    public class ClassWithInterface : ITestInterface
    {
    }

    public class ClassWithDerivedInterface : IDerivedTestInterface
    {
    }
}