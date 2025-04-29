namespace Web.Services
{
    using Application.Services.Contracts.User;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Security.Claims;

    public class CurrentUserService : ICurrentUser
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext?.User;

            if (user == null)
            {
                throw new InvalidOperationException("This request does not have an authenticated user.");
            }

            this.UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        }

        public string UserId { get; }
    }
}
