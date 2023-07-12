using Google.Apis.Http;
using Zeebe.Client.Accelerator.Abstractions;

namespace microservice;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IZeebeVariablesSerializer variablesSerializer;

    public Worker(ILogger<Worker> logger, IZeebeVariablesSerializer variablesSerializer)
    {
        this.variablesSerializer = variablesSerializer;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {



        var json = variablesSerializer.Serialize(new OutputVariables()
        {
            Output = new Output
            {
                Id = "1",
                Data = new Data()
                {
                    A = "b",
                    C = "d"
                }


            }
        });
        //_logger.LogInformation("json payload \n\n {json}", json);
        return Task.CompletedTask;



    }
}

public class OutputVariables
{
    public Output Output { get; set; }


}

public class Data
{
    public string A { get; set; }
    public string C { get; set; }
}

public class Output
{
    public string Id { get; set; }
    public Data Data { get; set; }
}


