using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UsersRestApi.Entities;

namespace UsersRestApi.Repositories
{
    public class BaseRepository<T> where T : BaseEntity
    {
        protected DbContext Context { get; set; }
        protected DbSet<T> Items { get; set; }

        public BaseRepository()
        {
            Context = new ProjectDBContext();
            Items = Context.Set<T>();
        }

        public List<T> GetAll()
        {
            IQueryable<T> query = Items;

            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = Items;

            return query.FirstOrDefault();
        }

        public int Count()
        {
            IQueryable<T> query = Items;

            return query.Count();
        }

        public void Delete(T item)
        {
            Items.Remove(item);
            Context.SaveChanges();
        }

        public void DeleteMany(T[] items)
        {
            Items.RemoveRange(items);
            Context.SaveChanges();
        }

        public int Add(T item)
        {
            Items.Add(item);
            Context.SaveChanges();

            return item.Id;
        }

    }
}
