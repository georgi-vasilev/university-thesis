namespace Application.Host.Commands.Update.Email
{
    public record UpdateEmailOutputModel
    {
        public UpdateEmailOutputModel(
            Guid id,
            string email)
        {
            Id = id;
            Email = email;
        }

        public Guid Id { get; set; }
        public string Email { get; init; } = default!;
    }
}
