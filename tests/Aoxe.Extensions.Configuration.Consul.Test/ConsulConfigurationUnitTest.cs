using Aoxe.Extensions.Configuration.Consul.Ini;
using Aoxe.Extensions.Configuration.Consul.Json;
using Aoxe.Extensions.Configuration.Consul.Xml;

namespace Aoxe.Extensions.Configuration.Consul.Test;

public class ConsulConfigurationUnitTest
{
    [Fact]
    public void ConfigurationTest()
    {
        var configBuilder = new ConfigurationBuilder().AddConsul(
            new ConsulClientConfiguration
            {
                Address = new Uri("http://localhost:8500"),
                Datacenter = "dc1",
            },
            "test-json",
            new JsonFlattener()
        );
        var configuration = configBuilder.Build();
        Assert.Equal("JsonApp", configuration["jsonApp:jsonName"]);
    }

    [Fact]
    public void ConfigurationIniTest()
    {
        var configBuilder = new ConfigurationBuilder().AddConsulIni(
            new ConsulClientConfiguration
            {
                Address = new Uri("http://localhost:8500"),
                Datacenter = "dc1",
            },
            "test-ini"
        );
        var configuration = configBuilder.Build();
        Assert.Equal("nestedStringValue", configuration["nestedSection:nestedStringKey"]);
    }

    [Fact]
    public void ConfigurationJsonTest()
    {
        var configBuilder = new ConfigurationBuilder().AddConsulJson(
            new ConsulClientConfiguration
            {
                Address = new Uri("http://localhost:8500"),
                Datacenter = "dc1",
            },
            "test-json"
        );
        var configuration = configBuilder.Build();
        Assert.Equal("JsonApp", configuration["jsonApp:jsonName"]);
    }

    [Fact]
    public void ConfigurationXmlTest()
    {
        var configBuilder = new ConfigurationBuilder().AddConsulXml(
            new ConsulClientConfiguration
            {
                Address = new Uri("http://localhost:8500"),
                Datacenter = "dc1",
            },
            "test-xml"
        );
        var configuration = configBuilder.Build();
        Assert.Equal("nestedStringValue", configuration["nestedObject:nestedStringKey"]);
    }
}
