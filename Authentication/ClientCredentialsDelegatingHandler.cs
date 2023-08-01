using Microservices.Common.Options;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace Microservices.Common.Authentication
{
    public class ClientCredentialsDelegatingHandler : DelegatingHandler
    {
        private readonly SsoOAuth2Options _ssoOAuth2Options;

        public ClientCredentialsDelegatingHandler(IOptionsMonitor<SsoOAuth2Options> ssoOAuth2Options)
        {
            _ssoOAuth2Options = ssoOAuth2Options.CurrentValue;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var jwt = request.Headers.Authorization?.Parameter;
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            //expired token
            if (token.ValidTo < DateTime.UtcNow)
            {
                //refresh token
                var newToken = TokenHelper.GetAccessTokenAsync(_ssoOAuth2Options).Result;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}