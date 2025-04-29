namespace Web.Features
{
    using Application.Order.Commands.Complete;
    using Application.Order.Commands.Create;
    using Application.Order.Commands.Purchase;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    public class OrdersController : ApiController
    {
        /// <summary>
        /// Create a new order (in state New).
        /// </summary>
        [HttpPost]
        public Task<ActionResult<CreateOrderOutputModel>> Create(CreateOrderCommand command) => Send(command);

        /// <summary>
        /// Mark an existing order as Completed.
        /// </summary>
        [HttpPut("complete")]
        public Task<ActionResult<OrderCompleteOutputModel>> Complete(OrderCompleteCommand command) => Send(command);

        /// <summary>
        /// Purchase tickets for an event:  
        /// Charges payment, creates an order, adds the ticket(s), and completes the order.
        /// </summary>
        [HttpPost("purchase")]
        public Task<ActionResult> Purchase(OrderPurchaseCommand command) => Send(command);

        //
        // TODO: once your queries are ready, you might add:
        //
        // [HttpGet]
        // public Task<ActionResult<List<OrderDto>>> List([FromQuery] GetOrdersByBuyerQuery q)
        //     => Send(q);
        //
        // [HttpGet("{id:guid}")]
        // public Task<ActionResult<OrderDto>> Details(Guid id)
        // {
        //     var q = new GetOrderDetailsQuery { OrderId = id };
        //     return Send(q);
        // }
        //
    }
}
