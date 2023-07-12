
using Zeebe;
using Zeebe.Client.Accelerator.Abstractions;
using Zeebe.Client.Accelerator.Attributes;
using Zeebe.Client.Accelerator.Extensions;

namespace microservice;


[JobType("shipgoods")]
public class ServiceTaskShipGoods : IAsyncZeebeWorker<ServiceInputVariable, ServiceOutputVariable>
{
    private readonly ILogger<ServiceTaskGenric> _logger;


    public ServiceTaskShipGoods(ILogger<ServiceTaskGenric> logger)
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

        return Task<ServiceOutputVariable>.FromResult(new ServiceOutputVariable { ShipmentStatus = true });
    }
}

