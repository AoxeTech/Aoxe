namespace Aoxe.Extensions.Configuration.Consul;

public class SimpleParser : IParser
{
    public Dictionary<string, string> Parse(string nodeName, byte[] bytes) =>
        new() { { nodeName, bytes.GetStringByUtf8() } };
}
