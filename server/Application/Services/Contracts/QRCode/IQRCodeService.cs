namespace Application.Services.Contracts.QRCode
{
    public interface IQRCodeService
    {
        byte[] GenerateQRCodeForTicket(Guid ticketId, Guid orderId, Guid eventId);
        string GenerateTicketValidationString(Guid ticketId, Guid orderId, Guid eventId);
    }
}
