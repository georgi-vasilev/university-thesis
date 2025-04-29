namespace Web.Features
{
    using Application.Host.Commands.Create;
    using Application.Host.Commands.Update.Email;
    using Application.Host.Commands.Update.InstagramHandler;
    using Application.Host.Commands.Update.PhoneNumber;
    using Application.Host.Commands.Update.Venue;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    public class HostsController : ApiController
    {
        /// <summary>
        /// Registers a new Host.
        /// </summary>
        [HttpPost]
        public Task<ActionResult<CreateHostOutputModel>> Create(CreateHostCommand command)
            => Send(command);

        /// <summary>
        /// Updates a Host’s email address.
        /// </summary>
        [HttpPut("email")]
        public Task<ActionResult<UpdateEmailOutputModel>> UpdateEmail(UpdateEmailCommand command)
            => Send(command);

        /// <summary>
        /// Updates a Host’s phone number.
        /// </summary>
        [HttpPut("phone-number")]
        public Task<ActionResult<UpdatePhoneNumberOutputModel>> UpdatePhoneNumber(UpdatePhoneNumberCommand command)
            => Send(command);

        /// <summary>
        /// Updates a Host’s Instagram handle.
        /// </summary>
        [HttpPut("instagram")]
        public Task<ActionResult<UpdateInstagramHandlerOutputModel>> UpdateInstagram(UpdateInstagramHandlerCommand command)
            => Send(command);

        /// <summary>
        /// Assigns or changes the Venue for a Host.
        /// </summary>
        [HttpPut("venue")]
        public Task<ActionResult<UpdateHostVenueOutputModel>> UpdateVenue(UpdateHostVenueCommand command)
            => Send(command);

        //
        // TODO: once you have queries:
        //
        // [HttpGet]
        // public Task<ActionResult<List<HostDto>>> List([FromQuery] GetHostsQuery q)
        //     => Send(q);
        //
        // [HttpGet("{id:guid}")]
        // public Task<ActionResult<HostDto>> Details([FromRoute] GetHostDetailsQuery q)
        //     => Send(q);
        //
    }
}
