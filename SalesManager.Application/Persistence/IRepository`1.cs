using Ardalis.Specification;

namespace SalesManager.Application.Persistence
{
    public interface IRepository<T> : IRepositoryBase<T>
        where T : class
    {
        IQueryable<TResult> ToPage<TResult>(ISpecification<T, TResult> specification);
    }
}
