namespace Application.Host.Commands.Update.InstagramHandler
{
    public record UpdateInstagramHandlerOutputModel
    {
        public UpdateInstagramHandlerOutputModel(
            Guid id,
            string instagramHandler)
        {
            Id = id;
            InstagramHandler = instagramHandler;
        }

        public Guid Id { get; set; }
        public string InstagramHandler { get; init; } = default!;
    }
}
