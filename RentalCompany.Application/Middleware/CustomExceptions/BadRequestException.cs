namespace RentalCompany.Application.Middleware.CustomExceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string? message) : base(message)
    {

    }
}
