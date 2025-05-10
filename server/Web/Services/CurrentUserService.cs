namespace Web.Services
{
    using Application.Services.Contracts.User;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Security.Claims;

    public class CurrentUserService : ICurrentUser
    {
        private readonly IHttpContextAccessor _accessor;

        public CurrentUserService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Guid UserId => Guid.Parse(_accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        public string Role => _accessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role)!;
        public Guid? HostId => GetGuidClaim("hostId");
        public Guid? BuyerId => GetGuidClaim("buyerId");

        private Guid? GetGuidClaim(string claimType)
        {
            var claim = _accessor.HttpContext?.User.FindFirst(claimType)?.Value;
            return string.IsNullOrEmpty(claim) ? null : Guid.Parse(claim);
        }
    }
}
