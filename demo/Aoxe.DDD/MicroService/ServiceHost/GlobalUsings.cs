// Global using directives

global using System;
global using System.Collections.Generic;
global using System.Threading;
global using System.Threading.Tasks;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.OpenApi.Models;
global using Repository;
global using Aoxe.NewtonsoftJson;
global using Aoxe.RabbitMQ;
global using Aoxe.RabbitMQ.Abstractions;
global using Aoxe.Serializer.Abstractions;
global using Aoxe.DDD;
global using Aoxe.DDD.Abstractions.Infrastructure.Messaging;
global using Aoxe.DDD.MessageBus.RabbitMQ;