# Zaaby.AspNetCore.Jil

Jil formatters for asp.net core

## QuickStart

### NuGet

Install-Package Zaaby.AspNetCore.Formatters.Jil

### Build Project

Create an asp.net core project and import reference in startup.cs

```CSharp
using Zaaby.AspNetCore.Formatters.Jil;
```

Modify the ConfigureServices like this

```CSharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers()
        .AddJil(jilOptions: new Options(dateFormat: DateTimeFormat.ISO8601,
            excludeNulls: true, includeInherited: true,
            serializationNameFormat: SerializationNameFormat.CamelCase));
}
```
