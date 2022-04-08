namespace Zaaby.AspNetCore.Formatters.Demo;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddNewtonsoftJson()
            .AddMvcOptions(options =>
            {
                var mediaTypeHeaderValue =
                    MediaTypeHeaderValue.Parse((StringSegment)"application/x-jil").CopyAsReadOnly();
                var serializer = new Zaabee.Jil.Serializer(new Options(dateFormat: DateTimeFormat.ISO8601,
                    excludeNulls: true, includeInherited: true,
                    serializationNameFormat: SerializationNameFormat.CamelCase));
                options.InputFormatters.Add(new ZaabyTextInputFormatter(mediaTypeHeaderValue, serializer));
                options.OutputFormatters.Add(new ZaabyTextOutputFormatter(mediaTypeHeaderValue, serializer));
                options.FormatterMappings.SetMediaTypeMappingForFormat("jil", mediaTypeHeaderValue);
            })
            // or you can
            // .AddJil(jilOptions: new Options(dateFormat: DateTimeFormat.ISO8601,
            //     excludeNulls: true, includeInherited: true,
            //     serializationNameFormat: SerializationNameFormat.CamelCase))
            .AddMvcOptions(options =>
            {
                var mediaTypeHeaderValue =
                    MediaTypeHeaderValue.Parse((StringSegment)"application/x-msgpack").CopyAsReadOnly();
                var serializer = new Zaabee.MsgPack.Serializer();
                options.InputFormatters.Add(new ZaabyInputFormatter(mediaTypeHeaderValue, serializer));
                options.OutputFormatters.Add(new ZaabyOutputFormatter(mediaTypeHeaderValue, serializer));
                options.FormatterMappings.SetMediaTypeMappingForFormat("msgpack", mediaTypeHeaderValue);
            })
            // or you can
            // .AddMsgPack()
            .AddMvcOptions(options =>
            {
                var mediaTypeHeaderValue =
                    MediaTypeHeaderValue.Parse((StringSegment)"application/x-protobuf").CopyAsReadOnly();
                var serializer = new Zaabee.Protobuf.Serializer();
                options.InputFormatters.Add(new ZaabyInputFormatter(mediaTypeHeaderValue, serializer));
                options.OutputFormatters.Add(new ZaabyOutputFormatter(mediaTypeHeaderValue, serializer));
                options.FormatterMappings.SetMediaTypeMappingForFormat("protobuf", mediaTypeHeaderValue);
            })
            // or you can
            // .AddProtobuf()
            .AddMvcOptions(options =>
            {
                var mediaTypeHeaderValue =
                    MediaTypeHeaderValue.Parse((StringSegment)"application/x-utf8json").CopyAsReadOnly();
                var serializer = new Zaabee.Utf8Json.Serializer();
                options.InputFormatters.Add(new ZaabyTextInputFormatter(mediaTypeHeaderValue, serializer));
                options.OutputFormatters.Add(new ZaabyTextOutputFormatter(mediaTypeHeaderValue, serializer));
                options.FormatterMappings.SetMediaTypeMappingForFormat("utf8json", mediaTypeHeaderValue);
            })
            // or you can
            // .AddUtf8Json()
            .AddMvcOptions(options =>
            {
                var mediaTypeHeaderValue =
                    MediaTypeHeaderValue.Parse((StringSegment)"application/x-zeroformatter").CopyAsReadOnly();
                options.InputFormatters.Add(
                    new ZaabyInputFormatter(mediaTypeHeaderValue, new Zaabee.ZeroFormatter.Serializer()));
                options.OutputFormatters.Add(new ZaabyOutputFormatter(mediaTypeHeaderValue,
                    new Zaabee.ZeroFormatter.Serializer()));
                options.FormatterMappings.SetMediaTypeMappingForFormat("zeroformatter", mediaTypeHeaderValue);
            })
            // or you can
            // .AddZeroFormatter()
            ;
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}