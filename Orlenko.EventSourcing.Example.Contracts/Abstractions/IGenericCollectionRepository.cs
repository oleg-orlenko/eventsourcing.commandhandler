using Orlenko.EventSourcing.Example.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Contracts.Abstractions
{
    public interface IGenericCollectionRepository<TEntity> where TEntity : Entity
    {
        Task<GenericCollection<TEntity>> GetAsync(CancellationToken cancellationToken = default);

        Task PersistAsync(GenericCollection<TEntity> collection, CancellationToken cancellationToken = default);
    }
}
