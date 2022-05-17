using System;
using System.Threading;
using System.Threading.Tasks;
using Agicap.Contracts;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace Publisher;


public class Worker : BackgroundService
{
    readonly IBus _bus;

    public Worker(IBus bus)
    {
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Guid id = Guid.NewGuid();
            DateTime now = DateTime.Now;
            Console.WriteLine($"Id: {id}, date: {now}");
            await _bus.Publish<IPaymentPaid>(new {PaymentId = id, PaidDate = new DateTimeOffset(now)}, stoppingToken);
            await Task.Delay(100, stoppingToken);
        }
    }
}