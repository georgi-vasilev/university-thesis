namespace Web.Features
{
    using Application.Order.Commands.Complete;
    using Application.Order.Commands.Fail;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Stripe;

    [ApiController]
    [Route("api/[controller]")]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly string _webhookSecret;

        public WebhookController(
            ILogger<WebhookController> logger,
            IConfiguration configuration,
            IMediator mediator)
        {
            _logger = logger;
            _configuration = configuration;
            _mediator = mediator;
            _webhookSecret = _configuration.GetValue<string>("Stripe:WebhookSecret")
                ?? throw new InvalidOperationException("Stripe webhook secret is required");
        }

        [HttpPost("stripe")]
        public async Task<IActionResult> HandleStripeWebhook()
        {
            try
            {
                var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                var stripeSignature = Request.Headers["Stripe-Signature"].FirstOrDefault();

                if (string.IsNullOrEmpty(stripeSignature))
                {
                    _logger.LogWarning("Missing Stripe signature header");
                    return BadRequest("Missing signature");
                }

                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    stripeSignature,
                    _webhookSecret);

                _logger.LogInformation("Received Stripe webhook: {EventType} for {ObjectId}",
                    stripeEvent.Type, stripeEvent.Id);

                switch (stripeEvent.Type)
                {
                    case "payment_intent.succeeded":
                        await HandlePaymentSucceeded(stripeEvent);
                        break;

                    case "payment_intent.payment_failed":
                        await HandlePaymentFailed(stripeEvent);
                        break;

                    case "payment_intent.canceled":
                        await HandlePaymentCanceled(stripeEvent);
                        break;

                    default:
                        _logger.LogInformation("Unhandled event type: {EventType}", stripeEvent.Type);
                        break;
                }

                return Ok();
            }
            catch (StripeException e)
            {
                _logger.LogError(e, "Stripe webhook signature verification failed");
                return BadRequest("Invalid signature");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error processing Stripe webhook");
                return StatusCode(500, "Internal server error");
            }
        }

        private async Task HandlePaymentSucceeded(Event stripeEvent)
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            if (paymentIntent == null)
            {
                _logger.LogWarning("PaymentIntent is null in payment_intent.succeeded event");
                return;
            }

            _logger.LogInformation("Processing payment success for PaymentIntent {PaymentIntentId}",
                paymentIntent.Id);

            var eventId = paymentIntent.Metadata.GetValueOrDefault("eventId");
            if (string.IsNullOrEmpty(eventId))
            {
                _logger.LogError("EventId not found in PaymentIntent metadata");
                return;
            }

            var buyerId = paymentIntent.Metadata.GetValueOrDefault("buyerId");

            var orderCompleteCommand = new OrderCompleteCommand(
                paymentIntent.Id,
                paymentIntent.Status,
                buyerId!,
                eventId,
                paymentIntent.Amount);
            var result = await _mediator.Send(orderCompleteCommand);

            if (result.IsError)
            {
                _logger.LogError("Failed to mark payment as completed for PaymentIntent {PaymentIntentId}: {Error}",
                    paymentIntent.Id, result.FirstError.Description);
            }
            else
            {
                _logger.LogInformation("Successfully marked payment as completed for PaymentIntent {PaymentIntentId}",
                    paymentIntent.Id);
            }
        }

        private async Task HandlePaymentFailed(Event stripeEvent)
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            if (paymentIntent == null)
            {
                _logger.LogWarning("PaymentIntent is null in payment_intent.payment_failed event");
                return;
            }

            _logger.LogInformation("Processing payment failure for PaymentIntent {PaymentIntentId}",
                paymentIntent.Id);

            var failureReason = paymentIntent.LastPaymentError?.Message ?? "Payment failed";
            var command = new MarkOrderFailedCommand(paymentIntent.Id, failureReason);

            var result = await _mediator.Send(command);
            if (result.IsError)
            {
                _logger.LogError("Failed to mark payment as failed for PaymentIntent {PaymentIntentId}: {Error}",
                    paymentIntent.Id, result.FirstError.Description);
            }
            else
            {
                _logger.LogInformation("Successfully marked payment as failed for PaymentIntent {PaymentIntentId}",
                    paymentIntent.Id);
            }
        }

        private async Task HandlePaymentCanceled(Event stripeEvent)
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            if (paymentIntent == null)
            {
                _logger.LogWarning("PaymentIntent is null in payment_intent.canceled event");
                return;
            }

            _logger.LogInformation("Processing payment cancellation for PaymentIntent {PaymentIntentId}",
                paymentIntent.Id);

            var command = new MarkOrderFailedCommand(paymentIntent.Id, "Payment was canceled");

            var result = await _mediator.Send(command);
            if (result.IsError)
            {
                _logger.LogError("Failed to mark canceled payment as failed for PaymentIntent {PaymentIntentId}: {Error}",
                    paymentIntent.Id, result.FirstError.Description);
            }
            else
            {
                _logger.LogInformation("Successfully marked canceled payment as failed for PaymentIntent {PaymentIntentId}",
                    paymentIntent.Id);
            }
        }
    }
}