# Zaaby.AspNetCore.Utf8Json

Utf8Json formatters for asp.net core

## QuickStart

### NuGet

Install-Package Zaaby.AspNetCore.Formatters.Utf8Json

### Build Project

Create an asp.net core project and import reference in startup.cs

```CSharp
using Zaaby.AspNetCore.Formatters.Utf8Json;
```

Modify the ConfigureServices like this

```CSharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers()
        .AddUtf8Json();
}
```
