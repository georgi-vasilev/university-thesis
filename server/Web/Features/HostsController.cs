namespace Web.Features
{
    using Application.Host.Commands.Update.Email;
    using Application.Host.Commands.Update.InstagramHandler;
    using Application.Host.Commands.Update.PhoneNumber;
    using Application.Host.Commands.Update.Venue;
    using Application.Host.Queries;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class HostsController : ApiController
    {
        [HttpGet("details/{Id:guid}", Name = nameof(GetHostDetailsQuery))]
        public Task<ActionResult<GetHostDetailsOutputModel>> Details([FromRoute] GetHostDetailsQuery query) => Send(query);


        [HttpPut("email")]
        [Authorize(Policy = "HostOnly")]
        public Task<ActionResult<UpdateEmailOutputModel>> UpdateEmail([FromBody] UpdateEmailCommand command)
            => Send(command);


        [HttpPut("phone-number")]
        [Authorize(Policy = "HostOnly")]
        public Task<ActionResult<UpdatePhoneNumberOutputModel>> UpdatePhoneNumber([FromBody] UpdatePhoneNumberCommand command)
            => Send(command);


        [HttpPut("instagram")]
        [Authorize(Policy = "HostOnly")]
        public Task<ActionResult<UpdateInstagramHandlerOutputModel>> UpdateInstagram([FromBody] UpdateInstagramHandlerCommand command)
            => Send(command);

        [HttpPut("venue")]
        [Authorize(Policy = "HostOnly")]
        public Task<ActionResult<UpdateHostVenueOutputModel>> UpdateVenue([FromBody] UpdateHostVenueCommand command)
            => Send(command);

    }
}
