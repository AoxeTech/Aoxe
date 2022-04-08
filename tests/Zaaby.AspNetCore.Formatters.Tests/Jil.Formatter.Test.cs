namespace Zaaby.AspNetCore.Formatters.Tests;

public partial class FormatterTest
{
    [Fact]
    public async Task JilFormatterTest()
    {
        var serializer = new Zaabee.Jil.Serializer(new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: true,
            includeInherited: true, serializationNameFormat: SerializationNameFormat.CamelCase));
        await TextFormatterAsync(serializer, "application/x-jil");
    }

    [Fact]
    public async Task JilFormatterNullTest()
    {
        var serializer = new Zaabee.Jil.Serializer(new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: true,
            includeInherited: true, serializationNameFormat: SerializationNameFormat.CamelCase));
        await TextFormatterNullAsync(serializer, "application/x-jil");
    }
}