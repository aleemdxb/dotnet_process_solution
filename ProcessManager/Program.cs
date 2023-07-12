

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace ProcessManager
{
    internal class Program
    {
        private static readonly string DemoProcessPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "demo-process.bpmn");
        private static readonly string ZeebeUrl = "localhost:26500";
        private static readonly string ProcessInstanceVariables = "{\"a\":\"123\"}";
        private static readonly string JobType = "payment-service";
        private static readonly string WorkerName = Environment.MachineName;
        private static readonly long WorkCount = 100L;

        public static async Task Main(string[] args)
        {
            // create zeebe client
            var client = ZeebeClient.Builder()
                .UseGatewayAddress(ZeebeUrl)
                .UsePlainText()
                .Build();

            // var topology = await client.TopologyRequest()
            //     .Send();
            // Console.WriteLine(topology);

            //Receive event from Rabbit MQ
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "order",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);


            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
            };
            channel.BasicConsume(queue: "order",
                                 autoAck: true,
                                 consumer: consumer);



            await client.NewPublishMessageCommand()
               .MessageName("receivegood")
               .CorrelationKey("123")
               .Variables("{\"EventCode\":2}")
               .Send();

            Console.WriteLine("message sent");
            Console.ReadLine();

            // deploy
            /*        var deployResponse = await client.NewDeployCommand()
                        .AddResourceFile(DemoProcessPath)
                        .Send();

                    // create process instance
                    var processDefinitionKey = deployResponse.Processes[0].ProcessDefinitionKey;

                    var processInstance = await client
                        .NewCreateProcessInstanceCommand()
                        .ProcessDefinitionKey(processDefinitionKey)
                        .Variables(ProcessInstanceVariables)
                        .Send();

                    await client.NewSetVariablesCommand(processInstance.ProcessInstanceKey).Variables("{\"wow\":\"this\"}").Local().Send();

                    for (var i = 0; i < WorkCount; i++)
                    {
                        await client
                            .NewCreateProcessInstanceCommand()
                            .ProcessDefinitionKey(processDefinitionKey)
                            .Variables(ProcessInstanceVariables)
                            .Send();
                    }

                    // open job worker
                    using (var signal = new EventWaitHandle(false, EventResetMode.AutoReset))
                    {
                        client.NewWorker()
                              .JobType(JobType)
                              .Handler(HandleJob)
                              .MaxJobsActive(5)
                              .Name(WorkerName)
                              .AutoCompletion()
                              .PollInterval(TimeSpan.FromSeconds(1))
                              .Timeout(TimeSpan.FromSeconds(10))
                              .Open();

                        // blocks main thread, so that worker can run
                        signal.WaitOne();
                    } */
        }

        private static void HandleJob(IJobClient jobClient, IJob job)
        {
            // business logic
            var jobKey = job.Key;

        }
    }
}