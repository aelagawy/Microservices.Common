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
        private readonly string _userId;
        private readonly string? _email;
        private readonly string? _name;
        private readonly string? _clientId;
        private readonly IEnumerable<string>? _roles;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            _userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";
            _email = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
            _name = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            _clientId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtClaimTypes.ClientId);
            _roles = _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();
        }

        public string? Email => _email?.ToLower();
        public string? NameAr => !string.IsNullOrEmpty(_name) && _name.Contains(';') ? _name.Split(';')[0] : null;
        public string? NameEn => !string.IsNullOrEmpty(_name) && _name.Contains(';') ? _name.Split(';')[1] : null;
        public string UserId => _userId.ToLower();
        public string? ClientId => _clientId?.ToLower();
        public UserType UserType => _roles.Contains("Admin") ? UserType.Administrator : UserType.Unknown;
        public IEnumerable<string> Roles => _roles ?? [];
        public bool IsAdministrator() => UserType == UserType.Administrator;
        public Task<string> Token()
        {
            throw new NotImplementedException();
        }
    }
}