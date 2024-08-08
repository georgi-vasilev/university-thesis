namespace Domain.Event
{
    using Domain.Event.Error;
    using ErrorOr;
    using Throw;

    internal class TimeRange
    {
        public TimeOnly Start { get; init; }
        public TimeOnly End { get; init; }

        public TimeRange(TimeOnly start, TimeOnly end)
        {
            Start = start.Throw().IfGreaterThanOrEqualTo(end);
            End = end;
        }

        public static ErrorOr<TimeRange> FromDateTimes(DateTime start, DateTime end)
        {
            if (start.Date != end.Date || start >= end)
            {
                return EventErrors.DefaultDateValueError;
            }

            return new TimeRange(TimeOnly.FromDateTime(start), TimeOnly.FromDateTime(end));
        }

        public bool OverlapsWith(TimeRange other)
        {
            if (Start >= other.End)
            {
                return false;
            }

            if (other.Start >= End)
            {
                return false;
            }

            return true;
        }
    }
}
