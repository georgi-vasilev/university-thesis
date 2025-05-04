namespace Application.Services.Contracts.User
{
    public interface ICurrentUser
    {
        Guid UserId { get; }
        string Role { get; }
        Guid? HostId { get; }
        Guid? BuyerId { get; }
    }
}
