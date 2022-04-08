namespace Zaaby.AspNetCore.Formatters.Tests;

public partial class FormatterTest
{
    [Fact]
    public async Task ProtobufFormatterTest()
    {
        var serializer = new Zaabee.Protobuf.Serializer();
        await StreamFormatterAsync(serializer, "application/x-protobuf");
    }

    [Fact]
    public async Task ProtobufFormatterNullTest()
    {
        var serializer = new Zaabee.Protobuf.Serializer();
        await StreamFormatterNullAsync(serializer, "application/x-protobuf");
    }
}