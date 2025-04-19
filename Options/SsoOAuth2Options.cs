namespace Microservices.Common.Options
{
    /// <summary>
    /// Configure locally, doesn't have any implementation in the Common Project
    /// This section should be mapped in the appsettings.json file in the consumer service.
    /// </summary>
    public class SsoOAuth2Options
    {
        public const string KEY = "SsoOAuth2";

        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
    }
}