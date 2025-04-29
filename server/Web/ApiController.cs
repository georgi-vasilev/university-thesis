namespace Web
{
    using Common;
    using ErrorOr;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    [ApiController]
    [Route("[controller]")]
    public abstract class ApiController : ControllerBase
    {
        public const string PathSeparator = "/";
        public const string Id = "{id}";

        private IMediator? mediator;
        protected IMediator Mediator
            => mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

        protected Task<ActionResult<TResult>> Send<TResult>(IRequest<ErrorOr<TResult>> request)
            => Mediator.Send(request).ToActionResult();

        protected Task<ActionResult> Send(IRequest<ErrorOr<Success>> request)
            => Mediator.Send(request).ToActionResult();
    }
}
