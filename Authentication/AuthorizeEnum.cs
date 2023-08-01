using Microservices.Common.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Microservices.Common.Authentication
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AuthorizeEnum : AuthorizeAttribute
    {
        public AuthorizeEnum(params object[] roles)
        {
            if (roles.Any(r => r.GetType().BaseType != typeof(Enum)))
                throw new ArgumentException(null, nameof(roles));

            Roles = string.Join(",", roles.Select(r => (UserType)r));
        }
    }
}