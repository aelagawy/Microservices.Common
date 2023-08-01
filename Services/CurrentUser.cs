using IdentityModel;
using Microservices.Common.Enums;
using Microservices.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Microservices.Common.Services
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string? _userId;
        private readonly string? _email;
        private readonly string? _clientId;
        private readonly IEnumerable<string>? _roles;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            _userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtClaimTypes.PreferredUserName);
            _email = _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtClaimTypes.Email);
            _clientId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtClaimTypes.ClientId);
            _roles = _httpContextAccessor.HttpContext?.User?.FindAll("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                .Select(x => x.Value)
                .ToList();
        }

        public string? Email => _email?.ToLower();
        public string? UserId => _userId?.ToLower();
        public string? ClientId => _clientId?.ToLower();
        public UserType UserType { get; } = UserType.Unknown;
        public IEnumerable<string> Roles => _roles ?? new List<string>();
        public Task<string> Token()
        {
            throw new NotImplementedException();
        }
    }
}