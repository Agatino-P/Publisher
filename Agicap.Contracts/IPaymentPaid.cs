using System;

// ReSharper disable once IdentifierTypo
// ReSharper disable once CheckNamespace
namespace Agicap.Contracts;

public interface IPaymentPaid : IIntegrationEvent
{
    Guid PaymentId { get; }

    DateTimeOffset PaidDate { get; }
}

public interface IIntegrationEvent
{
}