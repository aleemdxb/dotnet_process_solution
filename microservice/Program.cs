using microservice;
using Zeebe.Client.Accelerator.Extensions;

var builder = Host.CreateApplicationBuilder(args);
// Bootstrap Zeebe Integration
builder.Services.BootstrapZeebe(
    builder.Configuration.GetSection("ZeebeConfiguration"),
    typeof(Program).Assembly);


builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
