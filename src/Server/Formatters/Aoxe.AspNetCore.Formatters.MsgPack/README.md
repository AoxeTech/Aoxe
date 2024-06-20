# Aoxe.AspNetCore.MsgPack

Protobuf formatters for asp.net core

## QuickStart

### NuGet

Install-Package Aoxe.AspNetCore.Formatters.MsgPack

### Build Project

Create an asp.net core project and import reference in startup.cs

```CSharp
using Aoxe.AspNetCore.Formatters.MsgPack;
```

Modify the ConfigureServices like this

```CSharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers()
        .AddMsgPack();
}
```
