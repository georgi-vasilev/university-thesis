namespace Web.Features
{
    using Application.Order.Commands.Complete;
    using Application.Order.Commands.Create;
    using Application.Order.Commands.Purchase;
    using Application.Order.Common;
    using Application.Services.Contracts.User;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Stripe;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : ApiController
    {
        private readonly PaymentIntentService _paymentIntentService;
        private readonly ICurrentUser _currentUser;

        public OrdersController(PaymentIntentService paymentIntentService, ICurrentUser currentUser)
        {
            _paymentIntentService = paymentIntentService;
            _currentUser = currentUser;
        }

        [HttpPost]
        public Task<ActionResult<CreateOrderOutputModel>> Create([FromBody] CreateOrderCommand command) => Send(command);

        [HttpPut("complete")]
        public Task<ActionResult<OrderCompleteOutputModel>> Complete([FromBody] OrderCompleteCommand command) => Send(command);

        [HttpPost("purchase")]
        public Task<ActionResult> Purchase([FromBody] OrderPurchaseCommand command) => Send(command);

        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentRequest request)
        {
            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(request.Amount * 100),
                    Currency = "usd",
                    AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                    {
                        Enabled = true,
                    },
                    Metadata = new Dictionary<string, string>
                    {
                        { "buyerId", _currentUser.BuyerId!.Value.ToString() },
                        { "eventId", request.EventId },
                        { "ticketQuantity", request.TicketQuantity.ToString() },
                        { "ticketType", request.TicketType }
                    }
                };

                var paymentIntent = await _paymentIntentService.CreateAsync(options);

                return Ok(new CreatePaymentIntentResponse
                {
                    ClientSecret = paymentIntent.ClientSecret,
                    PaymentIntentId = paymentIntent.Id
                });
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = new { message = e.StripeError.Message } });
            }
        }
    }
}
