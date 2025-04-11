namespace Application.Host.Commands.Create
{
    public record CreateHostOutputModel
    {
        public CreateHostOutputModel(
            string hostName,
            string phoneNumber,
            string email,
            string instagramHandler)
        {
            HostName = hostName;
            PhoneNumber = phoneNumber;
            Email = email;
            InstagramHandler = instagramHandler;
        }

        public string HostName { get; init; } = default!;
        public string PhoneNumber { get; init; } = default!;
        public string Email { get; init; } = default!;
        public string InstagramHandler { get; init; } = default!;
    }
}
