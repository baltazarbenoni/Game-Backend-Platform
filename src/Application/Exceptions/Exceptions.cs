using Microsoft.AspNetCore.Http;
using System.Web;

namespace Application.Exceptions
{
    public class MyException : Exception
    {
        public MyException(string message) : base(message)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
        public int StatusCode { get; protected set; }
    }
    public class BadRequestException : MyException
    {
        public BadRequestException(string message) : base(message)
        {
            StatusCode = StatusCodes.Status400BadRequest; 
        }
    }
    public class UnauthorizedException : MyException
    {
        public UnauthorizedException(string message) : base(message)
        {
            StatusCode = StatusCodes.Status401Unauthorized; 
        }
    }
    public class NotFoundException : MyException
    {
        public NotFoundException(string message) : base(message)
        {
            StatusCode = StatusCodes.Status404NotFound;
        }
    }
    public class DatabaseException : MyException
    {
        public DatabaseException(string message) : base(message)
        {
            StatusCode = StatusCodes.Status409Conflict;
        }
    }
}