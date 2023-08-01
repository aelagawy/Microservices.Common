using IdentityModel.Client;
using Microservices.Common.Options;

namespace Microservices.Common.Authentication
{
    public static class TokenHelper
    {
        public static async Task<string> GetAccessTokenAsync(SsoOAuth2Options options)
        {
            var client = new HttpClient();
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = $"{options.Authority}/connect/token",
                ClientId = options.ClientId,
                ClientSecret = options.ClientSecret,
                Scope = options.Scope

            }).ConfigureAwait(false);
            tokenResponse.HttpResponse.EnsureSuccessStatusCode();

            return tokenResponse.AccessToken;
        }


        public static async Task<string> GetAccessTokenAsync(string authority, string clientId, string clientSecret, string scope)
        {
            var client = new HttpClient();
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = $"{authority}/connect/token",
                ClientId = clientId,
                ClientSecret = clientSecret,
                Scope = scope

            }).ConfigureAwait(false);
            tokenResponse.HttpResponse.EnsureSuccessStatusCode();

            return tokenResponse.AccessToken;
        }

        /// <summary>
        /// Not recommended approach on production, may be used locally or on test environment to enable login using postman or swagger
        /// </summary>
        /// <param name="authority"></param>
        /// <param name="clientId"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task<string> GetAccessTokenROPCAsync(string authority, string clientId, string username, string password)
        {
            var client = new HttpClient();
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = $"{authority}/connect/token",
                ClientId = clientId,
                UserName = username,
                Password = password

            }).ConfigureAwait(false);
            tokenResponse.HttpResponse.EnsureSuccessStatusCode();

            return tokenResponse.AccessToken;
        }
    }
}