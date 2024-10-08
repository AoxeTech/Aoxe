namespace AliceHost
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddJil()
                .AddMsgPack()
                .AddProtobuf()
                .AddUtf8Json()
                .AddZeroFormatter()
                .AddXmlSerializerFormatters();
            services
                .FromAssemblyOf<IAliceService>()
                .FromAssemblyOf<IBobService>()
                .FromAssemblyOf<ICarolService>()
                .FromAssemblyOf<AliceService>()
                .AddAoxeService<IService>()
                .AddAoxeService<ServiceAttribute>()
                .AddAoxeClient(
                    typeof(IService),
                    new Dictionary<string, string>
                    {
                        { typeof(IBobService).Namespace!, "http://localhost:5002" },
                        { typeof(ICarolService).Namespace!, "http://localhost:5003" }
                    },
                    options => options.UseJilFormatter()
                );
            // services.AddServiceRegistry(options =>
            // {
            //     var uri = new Uri("http://172.16.20.25:5001");
            //     options.ConsulAddress = "http://192.168.78.140:8500/";
            //     options.AgentServiceRegistration = new AgentServiceRegistration
            //     {
            //         ID = Guid.NewGuid().ToString(),
            //         Name = typeof(IAliceService).Namespace,
            //         Address = $"{uri.Scheme}://{uri.Host}",
            //         Port = uri.Port,
            //         Tags = new[] { "api" },
            //         Check = new AgentServiceCheck
            //         {
            //             DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
            //             Interval = TimeSpan.FromSeconds(30),
            //             HTTP = $"{uri.AbsoluteUri}HealthCheck"
            //         }
            //     };
            // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAoxe();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(
                    c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Alice API v1")
                );
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
