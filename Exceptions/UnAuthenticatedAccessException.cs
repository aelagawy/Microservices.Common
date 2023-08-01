namespace Microservices.Common.Exceptions
{
    public class UnAuthenticatedAccessException : Exception
    {
        public UnAuthenticatedAccessException()
            : base($"UnAuthenticated Operation!")
        {
        }
    }
}