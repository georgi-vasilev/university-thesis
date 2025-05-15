namespace Application.Events.Queries.GetEventsByHost
{
    using FluentValidation;

    public class GetEventsByHostQueryValidator : AbstractValidator<GetEventsByHostQuery>
    {
        public GetEventsByHostQueryValidator()
        {
            // TODO: add pagination validation
        }
    }
}
