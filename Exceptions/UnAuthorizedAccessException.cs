namespace Microservices.Common.Exceptions
{
    public class UnAuthorizedAccessException : Exception
    {
        public UnAuthorizedAccessException()
            : base($"Unauthorized Operation!")
        {
        }
    }
}