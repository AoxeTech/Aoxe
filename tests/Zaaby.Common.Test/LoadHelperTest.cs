using System;
using System.Diagnostics;
using System.Linq;
using AliceServices;
using BobServices;
using CarolServices;
using IAliceServices;
using IBobServices;
using ICarolServices;
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

            Assert.Equal(2, types0.Count);
            Assert.Single(types1);
            Assert.Contains(typeof(TestClassWithAttribute), types0.Select(p => p.ImplementationType));
            Assert.Contains(typeof(TestClassWithDerivedAttribute), types0.Select(p => p.ImplementationType));
            Assert.Contains(typeof(TestClassWithDerivedAttribute), types1.Select(p => p.ImplementationType));
        }

        [Fact]
        public void GetByBaseType()
        {
            var types0 = LoadHelper.GetByBaseType<ITestInterface>();
            var types1 = LoadHelper.GetByBaseType(typeof(IDerivedTestInterface));

            Assert.Equal(2, types0.Count);
            Assert.Contains(typeof(IDerivedTestInterface), types0.Select(p => p.InterfaceType));
            Assert.Contains(typeof(ClassWithInterface), types0.Select(p => p.ImplementationType));
            Assert.Contains(typeof(ClassWithDerivedInterface), types0.Select(p => p.ImplementationType));

            Assert.Single(types1);
            Assert.Null(types1[0].InterfaceType);
            Assert.Equal(typeof(ClassWithDerivedInterface), types1[0].ImplementationType);
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

            LoadHelper.LoadMode = LoadTypesMode.LoadByDirectory;
            
            sw.Restart();

            LoadHelper.LoadTypes();
            
            sw.Stop();
            var i1 = sw.ElapsedMilliseconds;

            Assert.True(LoadHelper.LoadScanTypes().Count > 0);
        }
    }

    public class TestAttribute : Attribute
    {
    }

    public class TestDerivedAttribute : TestAttribute
    {
    }

    [TestAttribute]
    public class TestClassWithAttribute
    {
    }

    [TestDerivedAttribute]
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