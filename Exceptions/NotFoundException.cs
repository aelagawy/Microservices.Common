namespace Microservices.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base($"Object Not found!")
        {
        }
    }
}