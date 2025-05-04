namespace Domain.Host
{
    public class OrganizedEventId
    {
        public Guid Value { get; private set; }

        private OrganizedEventId() { } // EF Core

        public OrganizedEventId(Guid value)
        {
            Value = value;
        }


        public override bool Equals(object? obj) =>
            obj is OrganizedEventId other && Value.Equals(other.Value);

        public override int GetHashCode() => Value.GetHashCode();
    }
}
