namespace Web.Features
{
    using Application.Host.Commands.Create;
    using Application.Host.Commands.Update.Email;
    using Application.Host.Commands.Update.InstagramHandler;
    using Application.Host.Commands.Update.PhoneNumber;
    using Application.Host.Commands.Update.Venue;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class HostsController : ApiController
    {
        // [HttpGet("{id:guid}")]
        // public Task<ActionResult<HostDto>> Details([FromRoute] GetHostDetailsQuery q)
        //     => Send(q);
        //

        [HttpPost]
        public Task<ActionResult<CreateHostOutputModel>> Create([FromBody] CreateHostCommand command)
            => Send(command);

        [HttpPut("email")]
        public Task<ActionResult<UpdateEmailOutputModel>> UpdateEmail([FromBody] UpdateEmailCommand command)
            => Send(command);


        [HttpPut("phone-number")]
        public Task<ActionResult<UpdatePhoneNumberOutputModel>> UpdatePhoneNumber([FromBody] UpdatePhoneNumberCommand command)
            => Send(command);


        [HttpPut("instagram")]
        public Task<ActionResult<UpdateInstagramHandlerOutputModel>> UpdateInstagram([FromBody] UpdateInstagramHandlerCommand command)
            => Send(command);

        [HttpPut("venue")]
        public Task<ActionResult<UpdateHostVenueOutputModel>> UpdateVenue([FromBody] UpdateHostVenueCommand command)
            => Send(command);

    }
}
