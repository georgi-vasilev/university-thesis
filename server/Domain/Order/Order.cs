namespace Domain.Order
{
    using Common;
    using Error;
    using ErrorOr;

    public class Order : IAggregateRoot
    {
        private readonly HashSet<Ticket> _tickets = new HashSet<Ticket>();
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;
        public Guid Id { get; private set; }
        public Guid BuyerId { get; private set; }
        public Guid EventId { get; private set; }
        public OrderStatus Status { get; private set; }
        public PaymentDetails? Payment { get; private set; }
        public IReadOnlyCollection<Ticket> Tickets => _tickets;

        internal Order(Guid buyerId, Guid? id = null)
        {
            BuyerId = buyerId;
            Status = OrderStatus.New;
            Id = id ?? Guid.NewGuid();
        }

        public ErrorOr<Success> AddTicket(Ticket ticket)
        {
            if (_tickets.Any(t => t.Id == ticket.Id))
            {
                return OrderError.TicketAlreadyAddedError;
            }

            _tickets.Add(ticket);

            //TODO: dispatch event
            return Result.Success;
        }

        public ErrorOr<Success> RemoveTicket(Guid ticketId)
        {
            var ticket = _tickets.FirstOrDefault(ticket => ticket.Id == ticketId);
            if (ticket is null)
            {
                return OrderError.TicketNotFoundError;
            }

            _tickets.Remove(ticket);

            //TODO: dispatch event
            return Result.Success;
        }

        public ErrorOr<Success> ChangeOrderStatus(OrderStatus newStatus)
        {
            if (newStatus == OrderStatus.New && Status == OrderStatus.Pending)
            {
                return OrderError.InvalidOrderStatusChangeOperationError;
            }

            if (Status == OrderStatus.Cancelled)
            {
                return OrderError.CannotChangeOrderStatusError;
            }

            if(newStatus == OrderStatus.Pending && Status == OrderStatus.Completed)
            {
                return OrderError.InvalidOrderStatusChangeOperationError;
            }

            Status = newStatus;

            return Result.Success;
        }

        public ErrorOr<Success> CompleteOrder()
        {
            if (_tickets.Count == 0)
            {
                return OrderError.NoTicketsInOrderError;
            }

            Status = OrderStatus.Completed;

            //TODO: dispatch event
            return Result.Success;
        }

        public ErrorOr<Success> MarkTicketAsSold(Guid ticketId, Guid attendeeId)
        {
            var ticket = _tickets.FirstOrDefault(t => t.Id == ticketId);
            if (ticket is null)
            {
                return OrderError.TicketNotFoundError;
            }

            var result = ticket.MarkAsSold(attendeeId);
            if (result.IsError)
            {
                return result.FirstError;
            }

            //TODO: dispatch event
            return Result.Success;
        }

        public ErrorOr<Success> UpdatePaymentDetails(PaymentDetails newPaymentDetails)
        {
            if (newPaymentDetails is null)
            {
                return PaymentError.InvalidPaymentDetailsError;
            }


            Payment = newPaymentDetails;

            //TODO: dispatch event
            return Result.Success;
        }

        public ErrorOr<Success> MarkPaymentAsCompleted(PaymentDetails completedPaymentDetails)
        {
            if (completedPaymentDetails is null)
            {
                return PaymentError.InvalidPaymentDetailsError;
            }

            if (completedPaymentDetails.Amount < 0)
            {
                return PaymentError.NegativeAmountError;
            }

            if (string.IsNullOrEmpty(completedPaymentDetails.TransactionId))
            {
                return PaymentError.InvalidTransactionIdError;
            }

            if (completedPaymentDetails.Status != PaymentStatus.Completed)
            {
                return PaymentError.PaymentStatusMismatchError;
            }

            Payment = completedPaymentDetails;
            Status = OrderStatus.Completed;

            //TODO: dispatch event
            return Result.Success;
        }

        public ErrorOr<Success> MarkPaymentAsFailed(PaymentDetails failedPaymentDetails)
        {
            if (failedPaymentDetails is null)
            {
                return PaymentError.InvalidPaymentDetailsError;
            }

            Payment = failedPaymentDetails;
            //TODO: dispatch event
            return Result.Success;
        }

        private void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        private void ClearDomainEvents() => _domainEvents.Clear();

    }
}
