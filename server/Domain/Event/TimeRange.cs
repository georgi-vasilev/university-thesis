namespace Domain.Event
{
    using Domain.Event.Error;
    using ErrorOr;
    using System.Globalization;

    public class TimeRange
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

        public override bool Equals(object obj)
        {
            return Equals(obj as TimeRange);
        }

        public bool Equals(TimeRange other)
        {
            if (other is null)
            {
                return false;
            }
            return Start.Equals(other.Start) && End.Equals(other.End);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, End);
        }

        public static bool operator ==(TimeRange left, TimeRange right)
        {
            if (left is null)
            {
                return right is null;
            }
            return left.Equals(right);
        }

        public static bool operator !=(TimeRange left, TimeRange right)
        {
            return !(left == right);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            var fmt = string.IsNullOrWhiteSpace(format) ? "yyyy-MM-dd HH:mm" : format;
            var provider = formatProvider ?? CultureInfo.InvariantCulture;
            return string.Format(provider, "{0:" + fmt + "} – {1:" + fmt + "}", Start, End);
        }
    }

}
