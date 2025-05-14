namespace Web.Features
{
    using Application.Common.Models;
    using Application.Events.Commands.Cancel;
    using Application.Events.Commands.Create;
    using Application.Events.Commands.Update;
    using Application.Events.Queries.GetEventDetails;
    using Application.Events.Queries.GetEvents;
    using Application.Events.Queries.GetEventsByHost;
    using Application.Events.Queries.GetEventsByVenue;
    using Application.Events.Queries.GetEventsInDateRange;
    using Application.Events.Queries.SearchEvents;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EventsController : ApiController
    {
        [HttpGet("all", Name = nameof(All))]
        public Task<ActionResult<PaginatedResult<GetEventsOutputModel>>> All([FromQuery] GetEventsQuery query) => Send(query);

        [HttpGet("filterByDate", Name = nameof(FilterByDate))]
        public Task<ActionResult<PaginatedResult<GetEventsInDateRangeOutputModel>>> FilterByDate([FromQuery] GetEventsInDateRangeQuery query) => Send(query);

        [HttpGet("search", Name = nameof(Search))]
        public Task<ActionResult<List<GetEventsOutputModel>>> Search([FromQuery] SearchEventsQuery query) => Send(query);

        [HttpGet("details/{Id:guid}", Name = nameof(Details))]
        public Task<ActionResult<GetEventDetailsOutputModel>> Details([FromRoute] GetEventDetailsQuery query) => Send(query);

        [HttpGet("host/{Id:guid}", Name = nameof(GetEventsByHost))]
        public Task<ActionResult<List<GetEventsOutputModel>>> GetEventsByHost([FromRoute] GetEventsByHostQuery query) => Send(query);

        [HttpGet("venue/{Id:guid}", Name = nameof(GetEventsByVenue))]
        public Task<ActionResult<List<GetEventsOutputModel>>> GetEventsByVenue([FromRoute] GetEventsByVenueQuery query) => Send(query);

        [HttpPost("create", Name = nameof(Create))]
        public Task<ActionResult<CreateEventOutputModel>> Create([FromBody] CreateEventCommand command) => Send(command);

        [HttpPut("{id:guid}")]
        public Task<ActionResult<UpdateCommandOutputModel>> Update([FromBody] UpdateEventCommand command) => Send(command);

        [HttpDelete("{id:guid}")]
        public Task<ActionResult> Cancel([FromBody] CancelEventCommand command) => Send(command);

    }
}
