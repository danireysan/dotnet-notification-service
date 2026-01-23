using dotnet_notification_service.Core.Domain.Entities;
using Funcky.Monads;

namespace dotnet_notification_service.Core.Application
{
    public interface IUseCase<T, in TParams>
        where T : notnull
    {
        public abstract Task<Either<Failure, T>> CallAsync(TParams @params);
    }
}