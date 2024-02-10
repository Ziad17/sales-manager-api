using SalesManager.Domain.Exceptions;

namespace SalesManager.Application.Base
{
    public class BaseService
    {
        public void CheckFound<T>(T element, string errorCode)
        {
            if (element is null)
                throw new DomainException(message: $"entity of type {nameof(element.GetType)} cannot be found",
                    code: errorCode);
        }
    }
}
