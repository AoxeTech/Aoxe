namespace Aoxe.Extensions.Configuration.Shared;

public interface IParser
{
    Dictionary<string, string> Parse(string nodeName, byte[] bytes);
}
