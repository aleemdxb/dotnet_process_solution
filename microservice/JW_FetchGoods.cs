
using Zeebe;
using Zeebe.Client.Accelerator.Abstractions;
using Zeebe.Client.Accelerator.Attributes;
using Zeebe.Client.Accelerator.Extensions;
using System.Text;
using RabbitMQ.Client;
namespace microservice;


[JobType("fetchgoods")]
public class ServiceTaskFetchGoods : IAsyncZeebeWorker<ServiceInputVariable, ServiceOutputVariable>
{
    private readonly ILogger<ServiceTaskGenric> _logger;


    public ServiceTaskFetchGoods(ILogger<ServiceTaskGenric> logger)
    {
        _logger = logger;

    }



    public Task<ServiceOutputVariable> HandleJob(ZeebeJob<ServiceInputVariable> job, CancellationToken cancellationToken)
    {
        // get process variables
        var variables = job.getVariables();
        // get custom headers
        ServiceHeaders headers = job.getCustomHeaders<ServiceHeaders>();

        // implement use case
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "order",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        const string message = "Request Order";
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: string.Empty,
                             routingKey: "order",
                             basicProperties: null,
                             body: body);


        return Task<ServiceOutputVariable>.FromResult(new ServiceOutputVariable { OrderRequestSent = true });
    }
}

