using System.Collections.Generic;

namespace Zaaby.Abstractions
{
    public interface IZaabyClient
    {
        IZaabyClient UseZaabyClient(string typeName, List<string> baseUrl);
        IZaabyClient UseZaabyClient(Dictionary<string, List<string>> baseUrls);
    }
}