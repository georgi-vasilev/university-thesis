namespace Web.Common
{
    using ErrorOr;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public static class ErrorOrExtensions
    {
        public static ActionResult<T> ToActionResult<T>(this ErrorOr<T> result)
        {
            if (!result.IsError)
            {
                return new OkObjectResult(result.Value);
            }

            var errorsByCode = result.Errors
                .GroupBy(e => e.Code)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.Description).ToArray()
                );

            var first = result.Errors[0];
            var details = new ValidationProblemDetails()
            {
                Title = first.Description,
                Status = first.Type == ErrorType.Validation
                         ? StatusCodes.Status400BadRequest
                         : StatusCodes.Status500InternalServerError
            };

            foreach (var kv in errorsByCode)
                details.Errors.Add(kv.Key, kv.Value);

            return first.Type == ErrorType.Validation
                ? new BadRequestObjectResult(details)
                : new ObjectResult(details) { StatusCode = details.Status };
        }

        public static async Task<ActionResult<T>> ToActionResult<T>(
            this Task<ErrorOr<T>> resultTask)
        {
            var result = await resultTask.ConfigureAwait(false);
            return result.ToActionResult();
        }


        public static ActionResult ToActionResult(this ErrorOr<Success> result)
        {
            if (!result.IsError)
            {
                return new OkResult();
            }

            var errorsByCode = result.Errors
                .GroupBy(e => e.Code)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.Description).ToArray()
                );

            var first = result.Errors[0];
            var details = new ValidationProblemDetails()
            {
                Title = first.Description,
                Status = first.Type == ErrorType.Validation
                         ? StatusCodes.Status400BadRequest
                         : StatusCodes.Status500InternalServerError
            };

            foreach (var kv in errorsByCode)
                details.Errors.Add(kv.Key, kv.Value);

            return first.Type == ErrorType.Validation
                ? (ActionResult)new BadRequestObjectResult(details)
                : new ObjectResult(details) { StatusCode = details.Status };
        }

        public static async Task<ActionResult> ToActionResult(
            this Task<ErrorOr<Success>> resultTask)
        {
            var result = await resultTask.ConfigureAwait(false);
            return result.ToActionResult();
        }
    }
}
