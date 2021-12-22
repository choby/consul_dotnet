using System.Net;
using System.Net.Sockets;
using consul_dotnet.consul;
using Microsoft.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Winton.Extensions.Configuration.Consul;
using Consul;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using consul_dotnet.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using consul_dotnet;

//Host.CreateDefaultBuilder(args)
//            .ConfigureWebHostDefaults(webBuilder =>
//            {
//                webBuilder.UseStartup<Startup>();
//                webBuilder.UseUrls("http://192.168.2.62:8810")

//            })
//            .Build()
//            .Run();



var startup = new Startup();

var builder = WebApplication.CreateBuilder(args);

startup.ConfigureServices(builder.Services);

var app = builder.Build();


startup.Configure(app, app.Environment, null);

app.Run($"http://192.168.2.62:8810");


