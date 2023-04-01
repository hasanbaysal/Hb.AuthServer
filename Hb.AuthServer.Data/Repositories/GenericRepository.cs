using Hb.AuthServer.Core.Repositories;
using Hb.AuthServer.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hb.AuthServer.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T: class
    {

        private readonly AppDbContext _context;
        protected DbSet<T> dbSet; 

        public GenericRepository(AppDbContext context)
        {
            _context = context;

            dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {

            return await dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var data= await dbSet.FindAsync(id);

            if (data!=null)
            {
                _context.Entry(data).State = EntityState.Detached;
            }

            return data;
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public T Update(T entity)
        {
         //dbSet.Update(entity);

            _context.Entry(entity).State = EntityState.Modified; 
            return entity;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> filter)
        {
            return dbSet.Where(filter);
        }
    }
}
