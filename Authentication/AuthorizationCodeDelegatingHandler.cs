using Microservices.Common.Interfaces;
using System.Net.Http.Headers;

namespace Microservices.Common.Authentication
{
    public class AuthorizationCodeDelegatingHandler : DelegatingHandler
    {
        private readonly ICurrentUser _currentUser;

        public AuthorizationCodeDelegatingHandler(ICurrentUser currentUser)
        {
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _currentUser.Token();

            //potentially refresh token here if it has expired etc.

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}