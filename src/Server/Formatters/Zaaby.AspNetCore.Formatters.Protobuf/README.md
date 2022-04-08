# Zaaby.AspNetCore.Protobuf

Protobuf formatters for asp.net core

## QuickStart

### NuGet

Install-Package Zaaby.AspNetCore.Formatters.Protobuf

### Build Project

Create an asp.net core project and import reference in startup.cs

```CSharp
using Zaaby.AspNetCore.Formatters.Protobuf;
```

Modify the ConfigureServices like this

```CSharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers()
        .AddProtobuf();
}
```
