
using Zeebe;
using Zeebe.Client.Accelerator.Abstractions;
using Zeebe.Client.Accelerator.Attributes;
using Zeebe.Client.Accelerator.Extensions;

namespace microservice;


[JobType("useexistingcreditcard")]
public class ServiceTaskGenric : IAsyncZeebeWorker<ServiceInputVariable, ServiceOutputVariable>
{
    private readonly ILogger<ServiceTaskGenric> _logger;


    public ServiceTaskGenric(ILogger<ServiceTaskGenric> logger)
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

        return Task<ServiceOutputVariable>.FromResult(new ServiceOutputVariable { PaymentStatus = true });
    }
}

public class ServiceInputVariable
{
    public string Name { get; set; }
    public decimal Price { get; set; }

}
public class ServiceOutputVariable
{

    public bool PaymentStatus { get; set; }

    public bool OrderRequestSent { get; set; }

    public bool ShipmentStatus { get; set; }





}
public class ServiceErrorVariable
{
    public string ErrorCode { get; set; }
    public string ErrorDescription { get; set; }

}

public class ServiceHeaders
{
    public string ServiceTaskGenric_Header_A { get; set; }
    public string ServiceTaskGenric_Header_B { get; set; }
}
