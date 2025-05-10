namespace Application.Events.Queries.GetEventsInDateRange
{
    using FluentValidation;

    public class GetEventsInDateRangeQueryValidator : AbstractValidator<GetEventsInDateRangeQuery>
    {
        public GetEventsInDateRangeQueryValidator()
        {
            RuleFor(x => x.StartDate)
                .NotNull()
                    .WithMessage("Start date cannot be null.")
                .LessThanOrEqualTo(x => x.EndDate)
                    .WithMessage("Start date must be less than or equal to end date.");

            RuleFor(x => x.EndDate)
                .NotNull()
                    .WithMessage("End date cannot be null.")
                .GreaterThanOrEqualTo(x => x.StartDate)
                    .WithMessage("End date must be greater than or equal to start date.");
        }
    }
}
