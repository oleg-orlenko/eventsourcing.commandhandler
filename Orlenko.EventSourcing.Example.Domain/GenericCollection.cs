using Orlenko.EventSourcing.Example.Domain.Events;
using Orlenko.EventSourcing.Example.Domain.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orlenko.EventSourcing.Example.Domain
{
    public abstract class GenericCollection<TEntity> where TEntity : Entity
    {
        protected readonly HashSet<TEntity> list;

        public IEnumerable<TEntity> All => list.AsEnumerable();

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        protected GenericCollection(IEnumerable<TEntity> items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            this.list = new HashSet<TEntity>(items);
        }

        public IOption<TEntity> GetById(Guid id)
        {
            var item = this.list.FirstOrDefault(x => x.Id == id);
            if (item is null)
                return new None<TEntity>();

            return new Some<TEntity>(item);
        }

        public virtual bool Add(TEntity item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            return this.list.Add(item);
        }

        public virtual bool Remove(TEntity item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            return this.list.Remove(item);
        }

        public virtual bool Update(TEntity newState)
        {
            var existingState = list.FirstOrDefault(x => x.Id == newState.Id);
            if (existingState is null)
                return false;

            if (!list.Remove(existingState))
                return false;

            return list.Add(newState);
        }

        public abstract IEnumerable<BaseEvent<TEntity>> GetChangesAfter(DateTime date);
    }
}
