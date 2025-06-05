using Microsoft.EntityFrameworkCore;
using Notify.Data;
using Notify.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Notify.Repositories
{
    public  abstract class Repository<K, T> : IRepository<K, T> where T:class
    {
        protected readonly NotifyContext _context;

        public Repository(NotifyContext context)
        {
            _context = context;
        }
        public async Task<T> Add(T item)
        {
            _context.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<T> Delete(K key)
        {
            var item = await Get(key);
            if (item != null)
            {
                _context.Remove(item);
                await _context.SaveChangesAsync();
                return item;
            }
            throw new Exception("No such item found for deleting");
        }

        public abstract Task<T> Get(K key);


        public abstract Task<IEnumerable<T>> GetAll();


        public async Task<T> Update(K key, T item)
        {
            var myItem = await Get(key);
            if (myItem != null)
            {
                _context.Entry(myItem).CurrentValues.SetValues(item);
                await _context.SaveChangesAsync();
                return item;
            }
            throw new Exception("No such item found for updation");
        }
    }
}