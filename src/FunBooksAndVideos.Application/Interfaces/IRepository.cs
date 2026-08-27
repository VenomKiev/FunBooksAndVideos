namespace FunBooksAndVideos.Application.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> Query();

        ValueTask AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    }
}
