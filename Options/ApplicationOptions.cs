namespace Microservices.Common.Options
{
    /// <summary>
    /// Configure locally, doesn't have any implementation in the Common Project
    /// This section should be mapped in the appsettings.json file in the consumer service.
    /// </summary>
    public class ApplicationOptions
    {
        public const string Application = "Application";
        public string PathBase { get; set; }
    }
}