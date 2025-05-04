namespace Web.Features
{
    using Application.Venues.Queries;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VenuesController : ApiController
    {
        [HttpGet("all")]
        public Task<ActionResult<IEnumerable<GetVenuesOutputModel>>> All([FromQuery] GetVenuesQuery query) => Send(query);

    }
}
