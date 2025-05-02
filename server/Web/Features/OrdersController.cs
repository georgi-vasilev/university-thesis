namespace Web.Features
{
    using Application.Order.Commands.Complete;
    using Application.Order.Commands.Create;
    using Application.Order.Commands.Purchase;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ApiController
    {
        [HttpPost]
        public Task<ActionResult<CreateOrderOutputModel>> Create([FromBody] CreateOrderCommand command) => Send(command);

        [HttpPut("complete")]
        public Task<ActionResult<OrderCompleteOutputModel>> Complete([FromBody] OrderCompleteCommand command) => Send(command);

        [HttpPost("purchase")]
        public Task<ActionResult> Purchase([FromBody] OrderPurchaseCommand command) => Send(command);

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
