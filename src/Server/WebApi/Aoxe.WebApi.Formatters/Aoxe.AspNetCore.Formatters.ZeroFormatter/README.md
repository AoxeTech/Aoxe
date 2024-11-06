# Aoxe.AspNetCore.ZeroFormatter

Protobuf formatters for asp.net core

## QuickStart

### NuGet

Install-Package Aoxe.AspNetCore.Formatters.ZeroFormatter

### Build Project

Create an asp.net core project and import reference in startup.cs

```CSharp
using Aoxe.AspNetCore.Formatters.ZeroFormatter;
```

Modify the ConfigureServices like this

```CSharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers()
        .AddZeroFormatter();
}
```
