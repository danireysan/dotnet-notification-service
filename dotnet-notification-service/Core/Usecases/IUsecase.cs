using dotnet_notification_service.Domain.Entities;
using Funcky.Monads;

namespace dotnet_notification_service.Core.Usecases
{
    public interface IUseCase<T, TParams>
        where T : notnull
    {
        public abstract Task<Either<Failure, T>> CallAsync(TParams @params);
    }
}