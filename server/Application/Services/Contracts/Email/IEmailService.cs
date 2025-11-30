namespace Application.Services.Contracts.Email
{
    using Domain.Order;

    public interface IEmailService
    {
        Task<bool> SendTicketEmailAsync(
            string recipientEmail,
            string recipientName,
            Order order,
            List<TicketEmailData> tickets,
            CancellationToken cancellationToken = default);
    }
}
