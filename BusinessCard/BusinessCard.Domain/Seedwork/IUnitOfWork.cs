namespace BusinessCard.Domain.Seedwork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }

}

