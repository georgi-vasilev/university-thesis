namespace Web.Features
{
    using Application.Events.Commands.Cancel;
    using Application.Events.Commands.Create;
    using Application.Events.Commands.Update;
    using Application.Events.Queries.GetEventDetails;
    using Application.Events.Queries.GetEvents;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ApiController
    {
        [HttpGet]
        public Task<ActionResult<List<GetEventsOutputModel>>> List([FromQuery] GetEventsQuery query) => Send(query);

        [HttpGet("{id:guid}")]
        public Task<ActionResult<GetEventDetailsOutputModel>> Details(Guid id) => Send(new GetEventDetailsQuery(id));

        [HttpPost]
        public Task<ActionResult<CreateEventOutputModel>> Create([FromBody] CreateEventCommand command) => Send(command);

        [HttpPut("{id:guid}")]
        public Task<ActionResult<UpdateCommandOutputModel>> Update([FromBody] UpdateEventCommand command) => Send(command);

        [HttpDelete("{id:guid}")]
        public Task<ActionResult> Cancel([FromBody] CancelEventCommand command) => Send(command);

    }
}
