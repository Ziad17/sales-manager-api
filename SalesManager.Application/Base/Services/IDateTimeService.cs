using System;

namespace SalesManager.Application.Base.Services
{
    public interface IDateTimeService
    {
        DateTime Now();

        DateTime Today();
    }
}
