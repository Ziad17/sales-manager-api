namespace SalesManager.Application.Base.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now()
        {
            return DateTime.UtcNow;
        }

        public DateTime Today()
        {
            return DateTime.Today;
        }
    }
}
