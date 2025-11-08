using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repository;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {

        private readonly StoreContext _dbcontext;

        public GenericRepository(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        #region Without Specification
        
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            //if (typeof(T) == typeof(Product))
            //    return (IReadOnlyList<T>)await _dbcontext.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync();
            //else
                return await _dbcontext.Set<T>().ToListAsync();
        }

      

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbcontext.Set<T>().FindAsync(id);

        }

       

        #endregion

        #region With Specification

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }




        private IQueryable<T> ApplySpecification (ISpecification<T> spec)
        {
            return SpecificationEvalutor<T>.GetQuery(_dbcontext.Set<T>(), spec);
        }

        public async Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task Add(T item)
         =>    await _dbcontext.Set<T>().AddAsync(item);

        public void Update(T item)
        => _dbcontext.Set<T>().Update(item);

        public void Delete(T item)
        => _dbcontext.Set<T>().Remove(item);

        #endregion




    }
}
