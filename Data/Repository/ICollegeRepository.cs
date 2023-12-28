using System.Linq.Expressions;

namespace CollegeApp.Data.Repository
{
    /*Implementing Common Repository pattern for entire application
     - This one is a application level repository thats why we should not see any Specific type like {eg: <Student>}
     - we should see only generic type like <T>
    */

    public interface ICollegeRepository<T>
    {
        Task<List<T>> GetAllAsync();

        Task<T> GetByIdAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false);

        Task<T> GetByNameAsync(Expression<Func<T, bool>> filter);

        Task<T> CreateAsync(T dbRecord);

        Task<T> UpdateAsync(T dbRecord);

        Task<bool> DeleteAsync(T dbRecord);
    }
}
