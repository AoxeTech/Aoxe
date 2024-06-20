namespace Aoxe.AspNetCore.Formatters.Tests;

public partial class FormatterTest
{
    [Fact]
    public async Task ProtobufFormatterTest()
    {
        var serializer = new Aoxe.Protobuf.Serializer();
        await StreamFormatterAsync(serializer, "application/x-protobuf");
    }

    [Fact]
    public async Task ProtobufFormatterNullTest()
    {
        var serializer = new Aoxe.Protobuf.Serializer();
        await StreamFormatterNullAsync(serializer, "application/x-protobuf");
    }
}
