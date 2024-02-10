using Ardalis.Specification;

namespace SalesManager.Domain
{
    /// <inheritdoc cref="BaseEntity{TKey}" />
    public abstract class BaseEntity : BaseEntity<Guid>, IEntity<Guid>
    {
    }
}
