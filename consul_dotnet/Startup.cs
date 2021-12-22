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

namespace consul_dotnet
{
	public class Startup
	{
        AppSettings appSettings;

		public void ConfigureServices(IServiceCollection services)
		{
			// Add services to the container.
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			services.AddControllers();

            this. appSettings = this.ConfigureAppSettings(services);

            this.ConfigureConsulConfig(services, appSettings!.ConsulServer.IP, appSettings!.ConsulServer.Port);

           
        }

		public void Configure(WebApplication app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
		{
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
           
            app.MapControllers();

            app.UseHttpsRedirection();

            //获取本机局域网ip
            var ipAddr = Dns.GetHostAddresses(Dns.GetHostName())
                .Where(x => x.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(x))
                .Select(x => x.ToString())
                .First();

            /***************************consul 注册中心**********************************/

            //实例化当前服务对象
            ServiceEntity serviceEntity = new ServiceEntity
            {
                IP = ipAddr,
                Port = Convert.ToInt32(appSettings.Service.Port),
                ServiceName = appSettings.Service.Name,
                ConsulIP = appSettings.ConsulServer.IP,
                ConsulPort = Convert.ToInt32(appSettings.ConsulServer.Port)
            };
            //将当前服务实例注册到consul
            app.RegisterConsul(app.Lifetime, serviceEntity);
           
        }

        private AppSettings ConfigureAppSettings(IServiceCollection services)
        {
            //读取appsettings配置
            var config = new ConfigurationBuilder()
                .AddJsonFile("./appsettings.json")
                /***************************consul 配置中心**********************************/
                .AddConsul("test-service/dbconfig", op =>
                {
                    op.ConsulConfigurationOptions = cco =>
                    {
                        cco.Address = new Uri("http://127.0.0.1:8500");
                    };
                    op.ReloadOnChange = true;
                })
                .Build();

            var appSettings = config.Get<AppSettings>();

            // config添加到依赖注入容器
            //通过该方法注入，可以通过IConfiguration获取
            services.AddSingleton(config);
            //builder.Configuration.AddConfiguration(config);

            //通过该方法注入后，可以通过IOptions<AppSettings>获取
            services.Configure<AppSettings>(config);
            return appSettings;
        }


        private void ConfigureConsulConfig(IServiceCollection services ,string consulIp,string consulPort)
        {
            //读取consul配置
            var config = new ConfigurationBuilder()
                /***************************consul 配置中心**********************************/
                .AddConsul("test-service/dbconfig", op =>
                {
                    op.ConsulConfigurationOptions = cco =>
                    {
                        //连接到consul配置中心
                        cco.Address = new Uri($"http://{consulIp}:{consulPort}");
                    };
                    op.ReloadOnChange = true;
                })
                .Build();

            services.Configure<DbConfig>(config);
        }

    }
}

