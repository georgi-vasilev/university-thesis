namespace Application.Services.Contracts
{
    using Domain.Common.ValueObject;
    using ErrorOr;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IPaymentService
    {
        Task<ErrorOr<PaymentResult>> ProcessPaymentAsync(Money amount, CancellationToken cancellationToken);
    }
}

