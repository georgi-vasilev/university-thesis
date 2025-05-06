using Application.Events.Queries.GetEventDetails;

namespace Application.Host.Queries
{
    public record GetHostDetailsOutputModel
    {
        public GetHostDetailsOutputModel(
            string fullName,
            string email,
            string instagramHandler,
            IEnumerable<GetEventDetailsOutputModel> organizedEvents)
        {
            FullName = fullName;
            Email = email;
            InstagramHandler = instagramHandler;
            OrganizedEvents = organizedEvents ?? new List<GetEventDetailsOutputModel>();
        }

        public string FullName { get; init; } = default!;
        public string Email { get; init; } = default!;
        public string InstagramHandler { get; init; } = default!;
        public IEnumerable<GetEventDetailsOutputModel> OrganizedEvents { get; init; } = new List<GetEventDetailsOutputModel>();
    }
}
