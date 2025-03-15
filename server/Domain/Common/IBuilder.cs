namespace Domain.Common
{
    using ErrorOr;

    public interface IBuilder<TEntity>
        where TEntity : IAggregateRoot
    {
        ErrorOr<TEntity> Build();
    }
}
