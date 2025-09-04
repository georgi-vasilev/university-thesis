namespace Infrastructure.Authentication
{
    using Application.Common;
    using Application.Common.Contracts;
    using Domain.Buyer;
    using Domain.Host;
    using Domain.Host.Builder;
    using Domain.Host.Repository;
    using ErrorOr;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationSettings _settings;
        private readonly IHostDomainRepository _hostRepository;
        private readonly IHostBuilder _hostBuilder;
        private readonly IContactInfoBuilder _contactInfoBuilder;

        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<ApplicationSettings> settings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _settings = settings.Value;
            _hostRepository = hostRepository;
            _hostBuilder = hostBuilder;
            _contactInfoBuilder = contactInfoBuilder;
        }

        public async Task<ErrorOr<string>> CreateHostUserAsync(
            string email,
            string password,
            Host host,
            CancellationToken cancellationToken)
        {
            var user = new ApplicationUser(email);
            user.BecomeHost(host);

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return Error.Validation("Identity", string.Join("; ", errors));
            }

            await _userManager.AddToRoleAsync(user, "Host");
            return GenerateJwtToken(user, "Host");
        }

        public async Task<ErrorOr<string>> CreateBuyerUserAsync(
            string email,
            string password,
            Buyer buyer,
            CancellationToken cancellationToken)
        {
            var user = new ApplicationUser(email);
            user.RegisterAsBuyer(buyer.Id);

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return Error.Validation("Identity", string.Join("; ", errors));
            }

            await _userManager.AddToRoleAsync(user, "Buyer");

            return GenerateJwtToken(user, "Buyer");
        }


        public async Task<ErrorOr<string>> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Error.Unauthorized("Invalid credentials");

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded) return Error.Unauthorized("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";

            return GenerateJwtToken(user, role);
        }

        private string GenerateJwtToken(ApplicationUser user, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_settings.Secret);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, role)
        };

            if (user.HostId.HasValue)
                claims.Add(new Claim("hostId", user.HostId.ToString()!));
            if (user.BuyerId.HasValue)
                claims.Add(new Claim("buyerId", user.BuyerId.ToString()!));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _settings.Issuer,
                Audience = _settings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}