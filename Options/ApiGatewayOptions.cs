namespace Microservices.Common.Options
{
    /// <summary>
    /// Configure locally, doesn't have any implementation in the Common Project
    /// This section should be mapped in the appsettings.json file in the consumer service.
    /// </summary>
    public class ApiGatewayOptions
    {
        public const string ApiGateway = "ApiGateway";

        public string BaseUrl { get; set; }
    }
}