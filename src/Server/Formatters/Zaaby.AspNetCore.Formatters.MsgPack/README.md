# Zaaby.AspNetCore.MsgPack

Protobuf formatters for asp.net core

## QuickStart

### NuGet

Install-Package Zaaby.AspNetCore.Formatters.MsgPack

### Build Project

Create an asp.net core project and import reference in startup.cs

```CSharp
using Zaaby.AspNetCore.Formatters.MsgPack;
```

Modify the ConfigureServices like this

```CSharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers()
        .AddMsgPack();
}
```
