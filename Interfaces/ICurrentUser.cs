using Microservices.Common.Enums;

namespace Microservices.Common.Interfaces
{
    public interface ICurrentUser
    {
        string? Email { get; }
        string UserId { get; }
        string? ClientId { get; }
        UserType UserType { get; }
        IEnumerable<string> Roles { get; }
        Task<string> Token();
    }
}