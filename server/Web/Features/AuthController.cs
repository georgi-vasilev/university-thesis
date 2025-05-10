namespace Web.Features
{
    using Application.Profile.Command.Login;
    using Application.Profile.Command.RegisterBuyer;
    using Application.Profile.Command.RegisterHost;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ApiController
    {
        [HttpPost(nameof(RegisterHost))]
        public Task<ActionResult<string>> RegisterHost([FromBody] RegisterHostUserCommand command) => Send(command);

        [HttpPost(nameof(RegisterBuyer))]
        public Task<ActionResult<string>> RegisterBuyer([FromBody] RegisterBuyerUserCommand command) => Send(command);

        [HttpPost(nameof(Login))]
        public Task<ActionResult<string>> Login([FromBody] LoginUserCommand command) => Send(command);
    }

}
