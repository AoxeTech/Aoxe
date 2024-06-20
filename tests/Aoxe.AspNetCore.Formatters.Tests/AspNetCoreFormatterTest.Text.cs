namespace Aoxe.AspNetCore.Formatters.Tests;

public partial class FormatterTest
{
    public async Task TextFormatterAsync(ITextSerializer textSerializer, string mediaType)
    {
        var dtos = GetDtos();

        var httpRequestMessage = CreateHttpRequestMessage(
            new StringContent(textSerializer.ToText(dtos), Encoding.UTF8, mediaType),
            mediaType
        );

        var response = await _server.CreateClient().SendAsync(httpRequestMessage);
        var result = textSerializer.FromText<List<TestDto>>(
            await response.Content.ReadAsStringAsync()
        );

        Assert.True(CompareDtos(dtos, result));
    }

    public async Task TextFormatterNullAsync(ITextSerializer textSerializer, string mediaType)
    {
        var httpRequestMessage = CreateHttpRequestMessage(
            new StringContent(textSerializer.ToText<List<TestDto>>(null), Encoding.UTF8, mediaType),
            mediaType
        );

        var response = await _server.CreateClient().SendAsync(httpRequestMessage);
        var result = textSerializer.FromText<List<TestDto>>(
            await response.Content.ReadAsStringAsync()
        );

        Assert.Null(result);
    }
}
