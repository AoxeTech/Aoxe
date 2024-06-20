namespace Aoxe.AspNetCore.Formatters.Tests;

public partial class FormatterTest
{
    public async Task StreamFormatterAsync(IStreamSerializer streamSerializer, string mediaType)
    {
        var dtos = GetDtos();

        var httpRequestMessage = CreateHttpRequestMessage(
            new StreamContent(streamSerializer.ToStream(dtos)),
            mediaType
        );

        var response = await _server.CreateClient().SendAsync(httpRequestMessage);
        var result = streamSerializer.FromStream<List<TestDto>>(
            await response.Content.ReadAsStreamAsync()
        );

        Assert.True(CompareDtos(dtos, result));
    }

    public async Task StreamFormatterNullAsync(IStreamSerializer streamSerializer, string mediaType)
    {
        var httpRequestMessage = CreateHttpRequestMessage(
            new StreamContent(streamSerializer.ToStream<List<TestDto>>(null)),
            mediaType
        );

        var response = await _server.CreateClient().SendAsync(httpRequestMessage);
        var result = streamSerializer.FromStream<List<TestDto>>(
            await response.Content.ReadAsStreamAsync()
        );

        Assert.Null(result);
    }
}
