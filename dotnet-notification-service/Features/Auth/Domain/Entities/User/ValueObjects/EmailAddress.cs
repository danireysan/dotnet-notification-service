using dotnet_notification_service.Core.Domain.Entities;
using Funcky.Monads;
using static System.Text.RegularExpressions.Regex;

namespace dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;

public sealed record EmailAddress
{
    public string Value { get; }

    private EmailAddress(string value) => Value = value;

    
    //! Only created for testing purposes do not use
    public static EmailAddress MockCreate(string value)
    
    {
        return new EmailAddress(value);
    }

    public static Either<Failure, EmailAddress> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            var failure = new UnprocessableEntityFailure
            {
                Message = "Email can't be empty or null"
            };
            return Either<Failure, EmailAddress>.Left(failure);
        }

        const string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        if (!IsMatch(value, pattern))
        {
            var failure = new UnprocessableEntityFailure
            {
                Message = "Invalid email address"
            };
            return Either<Failure, EmailAddress>.Left(failure);
        }


        return new EmailAddress(value);
    }
}