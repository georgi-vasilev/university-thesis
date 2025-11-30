namespace Application.Order.Commands.Complete
{
    using Domain.Event.Repository;
    using Domain.Event.Service;
    using Domain.Order;
    using Domain.Venue.Repository;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Services.Contracts;
    using Services.Contracts.Email;
    using Services.Contracts.QRCode;
    using Services.Contracts.User;

    public class OrderCompleteCommandHandler : IRequestHandler<OrderCompleteCommand, ErrorOr<OrderCompleteOutputModel>>
    {
        private readonly IEventOrderService _eventOrderService;
        private readonly ILogger<OrderCompleteCommandHandler> _logger;
        private readonly IEmailService _emailService;
        private readonly IQRCodeService _qrCodeService;
        private readonly IBuyerService _buyerService;
        private readonly IEventDomainRepository _eventRepository;
        private readonly IVenueDomainRepository _venueRepository;

        public OrderCompleteCommandHandler(
            IEventOrderService eventOrderService,
            ILogger<OrderCompleteCommandHandler> logger,
            IEmailService emailService,
            IQRCodeService qrCodeService,
            IEventDomainRepository eventRepository,
            IVenueDomainRepository venueRepository,
            IBuyerService buyerService)
        {
            _eventOrderService = eventOrderService;
            _logger = logger;
            _emailService = emailService;
            _qrCodeService = qrCodeService;
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
            _buyerService = buyerService;
        }

        public async Task<ErrorOr<OrderCompleteOutputModel>> Handle(OrderCompleteCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling OrderCompleteCommand for Order {OrderId}", request.TranscationId);
           
            var result = await _eventOrderService.CompleteOrderAsync(
                Guid.Parse(request.EventId),
                Guid.Parse(request.BuyerId),
                request.TranscationId,
                request.PaymentIntentStatus,
                request.Amount,
                cancellationToken);

            if (result.IsError)
            {
                return result.FirstError;
            }

            var order = result.Value;

            try
            {
                await SendTicketEmailAsync(order, Guid.Parse(request.EventId), cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send ticket email for order {OrderId}", order.Id);
            }

            return new OrderCompleteOutputModel(order.Id, order.Status);
        }

        private async Task SendTicketEmailAsync(Order order, Guid eventId, CancellationToken cancellationToken)
        {
            var user = await _buyerService.GetUserByIdAsync(order.BuyerId, cancellationToken);
            if (user is null)
            {
                _logger.LogWarning("User not found for order {OrderId}", order.Id);
                return;
            }

            var eventDetails = await _eventRepository.GetByIdAsync(eventId, cancellationToken);
            if (eventDetails is null)
            {
                _logger.LogWarning("Event not found for order {OrderId}", order.Id);
                return;
            }

            var venue = await _venueRepository.GetByIdAsync(eventDetails.VenueId, cancellationToken);
            if (venue is null)
            {
                _logger.LogWarning("Venue not found for order {OrderId}", order.Id);
                return;
            }

            var ticketEmailData = new List<TicketEmailData>();

            foreach (var ticket in order.Tickets)
            {
                var qrCodeImage = _qrCodeService.GenerateQRCodeForTicket(ticket.Id, order.Id, eventId);

                ticketEmailData.Add(new TicketEmailData(
                    ticket.Id,
                    eventDetails.Name,
                    venue.Name,
                    eventDetails.Date,
                    eventDetails.Time,
                    ticket.Type.ToString(),
                    ticket.Price.Amount,
                    qrCodeImage));
            }

            var emailSent = await _emailService.SendTicketEmailAsync(
                user.Email,
                user.FullName,
                order,
                ticketEmailData,
                cancellationToken);

            if (emailSent)
            {
                _logger.LogInformation("Successfully sent ticket email for order {OrderId}", order.Id);
            }
            else
            {
                _logger.LogWarning("Failed to send ticket email for order {OrderId}", order.Id);
            }
        }
    }
}