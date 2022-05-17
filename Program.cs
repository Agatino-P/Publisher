using System;
using System.Linq;
using System.Threading.Tasks;
using Agicap.Contracts;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Publisher
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program
    {
        private const string QueueName = "MyQueue";
        private const string ExchangeName = "MyExchange";

        public static async Task Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            IBus bus = host.Services.GetServices<IBus>().Single();

            string inputChar = "";
            Console.WriteLine("Q to exit");
            while (inputChar != "q")
            {
                Console.WriteLine("Hello");
                Guid id = Guid.NewGuid();
                DateTime now = DateTime.Now;
                Console.WriteLine($"Id: {id}, date: {now}");
                await bus.Publish<IPaymentPaid>(new {PaymentId = id, PaidDate = new DateTimeOffset(now)});
                inputChar = Console.ReadLine();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.AddMassTransit(config =>
                    {
                        config.UsingRabbitMq((_, cfg) =>
                        {
                            cfg.Host("localhost", "/", h =>
                            {
                                h.Username("admin");
                                h.Password("admin");
                            });
                            cfg.Publish<IIntegrationEvent>(x => x.Exclude = true);
                            cfg.Message<IPaymentPaid>(x => x.SetEntityName(QueueName));
                        });
                    });
                    services.AddHostedService<Worker>();
                });
    }
}