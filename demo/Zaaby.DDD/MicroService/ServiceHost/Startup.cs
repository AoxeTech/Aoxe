using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Zaabee.NewtonsoftJson;
using Zaabee.RabbitMQ;
using Zaabee.RabbitMQ.Abstractions;
using Zaabee.Serializer.Abstractions;
using Zaaby.DDD;
using Zaaby.DDD.Abstractions.Infrastructure.MessageBus;
using Zaaby.DDD.MessageBus.RabbitMQ;

namespace ServiceHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MicroService", Version = "v1" });
            });

            //注册DDD各层
            services.AddDDD()
                //注册DbContext
                .AddDbContext(Configuration)
                //注册序列化器用于包装需要持久化的事件
                .AddSingleton<ITextSerializer, ZaabeeSerializer>()
                //注册RabbitMQ以用于消息总线
                .AddSingleton<IMessageBus, MessageBus>()
                .AddSingleton<IZaabeeRabbitMqClient>(_ =>
                    new ZaabeeRabbitMqClient(new ZaabeeRabbitMqOptions
                    {
                        AutomaticRecoveryEnabled = true,
                        HeartBeat = TimeSpan.FromMinutes(1),
                        NetworkRecoveryInterval = new TimeSpan(60),
                        Hosts = new List<string> { "192.168.78.150" },
                        UserName = "admin",
                        Password = "123",
                        Serializer = new ZaabeeSerializer()
                    }))
                // .AddHostedService<DomainEventBackgroundService>()
                ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MicroService v1"));
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<UowMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}