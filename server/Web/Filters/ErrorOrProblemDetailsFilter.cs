namespace Web.Filters
{
    using ErrorOr;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Linq;

    public class ErrorOrProblemDetailsFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult or &&
                or.Value is ErrorOr<object> errorOr)
            {
                context.Result = errorOr.Match<IActionResult>(
                    value => new OkObjectResult(value),
                    errors =>
                    {
                        var pd = new ValidationProblemDetails(
                            errors
                                .Where(e => e.Type == ErrorType.Validation)
                                .GroupBy(e => e.Code)
                                .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray())
                        )
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Title = "One or more validation errors occurred"
                        };
                        return new ObjectResult(pd) { StatusCode = pd.Status };
                    }
                );
            }
        }
    }
}