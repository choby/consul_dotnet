using consul_dotnet.consul;
using Microsoft.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseHttpsRedirection();

IConfiguration Configuration = new ConfigurationBuilder().AddJsonFile("./appsettings.json").Build();
Console.Write(Configuration);
// register this service
ServiceEntity serviceEntity = new ServiceEntity
{
    IP = "192.168.2.62",
    Port = Convert.ToInt32(Configuration["Service:Port"]),
    ServiceName = Configuration["Service:Name"],
    ConsulIP = Configuration["Consul:IP"],
    ConsulPort = Convert.ToInt32(Configuration["Consul:Port"])
};


app.RegisterConsul(app.Lifetime, serviceEntity);

app.Run("http://192.168.2.62:8810");


