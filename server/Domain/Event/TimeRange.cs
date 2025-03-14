namespace Domain.Event
{
    using Domain.Event.Error;
    using ErrorOr;

    internal class TimeRange
    {
        public DateTime Start { get; init; }
        public DateTime End { get; init; }

        internal TimeRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public static ErrorOr<TimeRange> FromDateTimes(DateTime start, DateTime end)
        {
            if (start >= end)
            {
                return EventErrors.DefaultDateValueError;
            }

            return new TimeRange(start, end);
        }

        public bool OverlapsWith(TimeRange other)
        {
            return Start < other.End && other.Start < End;
        }
    }

}
