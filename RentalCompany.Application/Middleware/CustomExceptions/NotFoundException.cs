﻿namespace RentalCompany.Application.Middleware.CustomExceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {

    }
}

