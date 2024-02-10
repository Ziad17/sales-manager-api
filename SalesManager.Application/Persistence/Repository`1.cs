using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;

namespace SalesManager.Application.Persistence
{
    public class Repository<T> : RepositoryBase<T>, IRepository<T>
        where T : class
    {
        public Repository(BaseContext context)
            : base(context)
        {
        }

        public IQueryable<TResult> ToPage<TResult>(ISpecification<T, TResult> specification)
        {
            return ApplySpecification(specification);
        }
    }
}
