namespace Web.Features
{
    using Application.Events.Commands.Cancel;
    using Application.Events.Commands.Create;
    using Application.Events.Commands.Update;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ApiController
    {
        [HttpPost]
        public Task<ActionResult<CreateEventOutputModel>> Create([FromBody] CreateEventCommand command) => Send(command);

        [HttpPut("{id:guid}")]
        public Task<ActionResult<UpdateCommandOutputModel>> Update(UpdateEventCommand command) => Send(command);

        [HttpDelete("{id:guid}")]
        public Task<ActionResult> Cancel([FromBody] CancelEventCommand command) => Send(command);

        //
        // TODO:
        //
        // [HttpGet]
        // public Task<ActionResult<List<EventDto>>> List([FromQuery] GetEventsQuery q)
        //     => Send(q);
        //
        // [HttpGet("{id:guid}")]
        // public Task<ActionResult<EventDto>> Details(Guid id)
        // {
        //     var q = new GetEventDetailsQuery { EventId = id };
        //     return Send(q);
        // }
        //
    }
}
