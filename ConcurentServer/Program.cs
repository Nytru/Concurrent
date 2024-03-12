using ConcurentServer.Endpoints;
using ConcurentServer.Extensions;
using ConcurentServer.Options;

var builder = WebApplication.CreateBuilder(args);
// var maxValue = ValidateArgs(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var services = builder.Services;
// services.Configure<SemaphoreOptions>(options => options.MaxValue = maxValue);
services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .ConfigureServiceOptions(builder.Configuration)
    .AddServices();

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
return;


int ValidateArgs(string[] args)
{
    if (args.Length != 1)
    {
        throw new ArgumentException("args must be exactly of lenght 1", nameof(args));
    }

    return int.Parse(args.First());
}