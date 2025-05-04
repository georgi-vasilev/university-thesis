namespace Domain.Buyer.Builder
{
    using Common;

    public interface IBuyerBuilder : IBuilder<Buyer>
    {
        IBuyerBuilder WithName(string firstName, string lastName);
        IBuyerBuilder WithEmail(string email);
    }
}
