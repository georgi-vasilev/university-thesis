namespace Application.Host.Commands.Update.PhoneNumber
{
    public record UpdatePhoneNumberOutputModel
    {
        public UpdatePhoneNumberOutputModel(
            Guid id,
            string phoneNumber)
        {
            Id = id;
            PhoneNumber = phoneNumber;
        }

        public Guid Id { get; set; }
        public string PhoneNumber { get; init; } = default!;
    }
}
