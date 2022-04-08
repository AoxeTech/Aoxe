namespace Zaaby.AspNetCore.Formatters.Tests;

public partial class FormatterTest
{
    [Fact]
    public async Task ZeroFormatterTest()
    {
        var serializer = new Zaabee.ZeroFormatter.Serializer();
        await StreamFormatterAsync(serializer, "application/x-zeroformatter");
    }

    [Fact]
    public async Task ZeroFormatterNullTest()
    {
        var serializer = new Zaabee.ZeroFormatter.Serializer();
        await StreamFormatterNullAsync(serializer, "application/x-zeroformatter");
    }
}