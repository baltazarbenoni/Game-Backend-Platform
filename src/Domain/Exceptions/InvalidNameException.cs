namespace Domain.Exceptions
{
    public class InvalidNameException : Exception
    {
        public InvalidNameException(string displayName) : base($"The name {displayName} is invalid. Name must contain at least 3 characters"){}
    }
}