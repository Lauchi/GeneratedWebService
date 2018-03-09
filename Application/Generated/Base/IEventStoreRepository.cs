using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Application
{
    public interface IEventStoreRepository
    {
        Task AddEvents(List<DomainEventBase> domainEvents);
    }
}