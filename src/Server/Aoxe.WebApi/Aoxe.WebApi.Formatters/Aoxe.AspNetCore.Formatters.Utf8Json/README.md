# Aoxe.AspNetCore.Utf8Json

Utf8Json formatters for asp.net core

## QuickStart

### NuGet

Install-Package Aoxe.AspNetCore.Formatters.Utf8Json

### Build Project

Create an asp.net core project and import reference in startup.cs

```CSharp
using Aoxe.AspNetCore.Formatters.Utf8Json;
```

Modify the ConfigureServices like this

```CSharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers()
        .AddUtf8Json();
}
```
