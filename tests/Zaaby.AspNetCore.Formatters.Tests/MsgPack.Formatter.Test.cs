namespace Zaaby.AspNetCore.Formatters.Tests;

public partial class FormatterTest
{
    [Fact]
    public async Task MsgPackFormatterTest()
    {
        var serializer = new Zaabee.MsgPack.Serializer();
        await StreamFormatterAsync(serializer, "application/x-msgpack");
    }

    [Fact]
    public async Task MsgPackFormatterNullTest()
    {
        var serializer = new Zaabee.MsgPack.Serializer();
        await StreamFormatterNullAsync(serializer, "application/x-msgpack");
    }
}