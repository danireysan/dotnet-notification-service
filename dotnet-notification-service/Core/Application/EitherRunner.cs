using dotnet_notification_service.Core.Domain.Entities;
using Funcky.Monads;

namespace dotnet_notification_service.Core.Application;


public static class EitherRunner
{
    public static async Task<Either<Failure, T>> RunAsync<T>(Func<Task<Either<Failure, T>>> happyPath)
    {
        Failure?  failure = null;
        try
        {
            return await happyPath();
        }
        catch (Exception e)
        {
            return Either<Failure, T>.Left(new ServerFailure { Message = e.Message });
        }
    }
    
}