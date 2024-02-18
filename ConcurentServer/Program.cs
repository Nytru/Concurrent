using ConcurentServer.Endpoints;
using ConcurentServer.Extensions;
using ConcurentServer.Services;
using Coravel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var services = builder.Services;
services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddServices()
    .ConfigureServiceOptions(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.MapControllers();
app.UseHttpsRedirection();
var group = app.MapGroup("/api");
group.RegisterApiEndpoints();

app.Run();
