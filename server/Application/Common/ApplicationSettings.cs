namespace Application.Common
{
    public class ApplicationSettings
    {
        public string Secret { get; init; } = default!;
        public string Issuer { get; init; } = default!;
        public string Audience { get; init; } = default!;
    }
}
