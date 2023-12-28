
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CollegeApp.Data.Repository
{
    public class CollegeRepository<T> : ICollegeRepository<T> where T : class
    {
        //Injected DBContext
        private readonly CollegeDBContext _dbContext;

        //here we are creating common table instead of specific table like eg: Students
        private DbSet<T> _dbSet;
        public CollegeRepository(CollegeDBContext dBContext) 
        {
            _dbContext = dBContext;
            _dbSet = _dbContext.Set<T>(); //common Table
        }

        public async Task<T> CreateAsync(T dbRecord)
        {
            _dbSet.Add(dbRecord);
            await _dbContext.SaveChangesAsync();
            return dbRecord;
        }

        public async Task<bool> DeleteAsync(T dbRecord)
        {
            _dbSet.Remove(dbRecord);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T , bool>> filter, bool useNoTracking = false)
        {
            /*we can not use a specific members or properties eg: student.Id in a common repository
             *We have used the predicates or delegates here eg:{Expression<Func<T, bool>> filter}
             */
            if (useNoTracking)
            {
                return await _dbSet.AsNoTracking().Where(filter).FirstOrDefaultAsync();
            }
            else
            {
                return await _dbSet.Where(filter).FirstOrDefaultAsync();
            }
        }

        public async Task<T> GetByNameAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).FirstOrDefaultAsync(); //Instead of "Equel" use "Contains" for partially match the name
        }

        public async Task<T> UpdateAsync(T dbRecord)
        {
            _dbContext.Update(dbRecord);
            await _dbContext.SaveChangesAsync();
            return dbRecord;

        }
    }
}
