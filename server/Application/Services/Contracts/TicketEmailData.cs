using Domain.Event;

namespace Application.Services.Contracts
{
    public record TicketEmailData(
       Guid TicketId,
       string EventName,
       string VenueName,
       DateOnly EventDate,
       TimeRange EventTime,
       string TicketType,
       decimal Price,
       byte[] QRCodeImage);
}
