using System;
using dotnet_notification_service.Core.Domain.Entities;
using Funcky.Monads;

namespace dotnet_notification_service.Core;

public static class EitherExtensions
{
    /// <summary>
    /// Attempts to extract the Right value from an Either, returning null and capturing the Left failure if present.
    /// Preserves the previous behavior of the in-file helper: returns default when the Either is Left and
    /// sets the out failure to the Left value.
    /// </summary>
    public static T? UnwrapWithNull<T>(this Either<Failure, T> either, out Failure? failure)
    {
        failure = null;
        Failure? tempFailure = null;
        try
        {
            var value = either.Match(
                left =>
                {
                    tempFailure = left;
                    throw new Exception(left.Message);
                },
                right => right
            );

            failure = tempFailure;
            return value;
        }
        catch (Exception)
        {
            failure = tempFailure;
            return default;
        }
    }
}
