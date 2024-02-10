using Ardalis.Specification;
using SalesManager.Domain.Entities;

namespace SalesManager.Application.Specifications.Admins
{
    public sealed class AdminSpecifications : Specification<Admin>, ISingleResultSpecification<Admin>
    {
        public AdminSpecifications()
        {
        }

        public AdminSpecifications Id(Guid id)
        {
            Query.AsNoTracking();
            Query.Where(c => c.Id == id);
            return this;
        }
    }
}
