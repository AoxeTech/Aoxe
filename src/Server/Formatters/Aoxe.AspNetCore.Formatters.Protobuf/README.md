# Aoxe.AspNetCore.Protobuf

Protobuf formatters for asp.net core

## QuickStart

### NuGet

Install-Package Aoxe.AspNetCore.Formatters.Protobuf

### Build Project

Create an asp.net core project and import reference in startup.cs

```CSharp
using Aoxe.AspNetCore.Formatters.Protobuf;
```

Modify the ConfigureServices like this

```CSharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers()
        .AddProtobuf();
}
```
