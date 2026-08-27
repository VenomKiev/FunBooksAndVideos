using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
