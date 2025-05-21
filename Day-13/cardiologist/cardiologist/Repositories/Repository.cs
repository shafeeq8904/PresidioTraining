using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cardiologist.Exceptions;
using cardiologist.Interfaces;

namespace cardiologist.Repositories
{
    public abstract class Repository<K, T> : IRepositor<K, T> where T : class
    {
        protected List<T> _items = new List<T>();
        protected abstract K GenerateID();
        public abstract ICollection<T> GetAll();
        public abstract T GetById(K id);

        public T Add(T item)
        {
            var id = GenerateID();
            var property = typeof(T).GetProperty("Id");
            if (property != null)
            {
                property.SetValue(item, id);
            }

            if (_items.Contains(item))
            {
                throw new DuplicateEntityException("Entity already exists.");
            }

            _items.Add(item);
            return item;
        }

        public T Delete(K id)
        {
            var item = GetById(id);
            _items.Remove(item);
            return item;
        }

        public T Update(T item)
        {
            var existing = _items.FirstOrDefault(i => i.Equals(item));
            if (existing == null)
            {
                throw new KeyNotFoundException("Entity not found.");
            }

            var index = _items.IndexOf(existing);
            _items[index] = item;
            return item;
        }
    }
}
