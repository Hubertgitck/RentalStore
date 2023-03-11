namespace RentalCompany.Application.Middleware.CustomExceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(string? message) : base(message)
    {

    }
}
