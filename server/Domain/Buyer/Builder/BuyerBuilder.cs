namespace Domain.Buyer.Builder
{
    using ErrorOr;

    public class BuyerBuilder : IBuyerBuilder
    {
        private string? _firstName;
        private string? _lastName;
        private string? _email;

        public IBuyerBuilder WithName(string firstName, string lastName)
        {
            _firstName = firstName;
            _lastName = lastName;
            return this;
        }

        public IBuyerBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public ErrorOr<Buyer> Build()
        {
            if (string.IsNullOrWhiteSpace(_firstName) || string.IsNullOrWhiteSpace(_lastName))
                return Error.Validation("Name", "First and Last name are required.");

            if (string.IsNullOrWhiteSpace(_email))
                return Error.Validation("Email", "Email is required.");

            return new Buyer(_firstName, _lastName, _email);
        }
    }

}
