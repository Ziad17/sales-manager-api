using Ardalis.Specification;

namespace SalesManager.Application.Base
{
    /// <inheritdoc cref="BaseEntity{TKey}" />
    public abstract class BaseEntity : BaseEntity<Guid>, IEntity<Guid>
    {
    }
}
