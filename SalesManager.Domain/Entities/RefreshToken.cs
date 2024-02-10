namespace SalesManager.Domain.Entities
{
    public class RefreshToken : ValueObject
    {
        public RefreshToken(string value, DateTime expireAt, DateTime createdAt)
        {
            SetExpireAt(expireAt);
            SetValue(value);
            SetCreatedAt(createdAt);
        }

        protected RefreshToken()
        {
        }

        public DateTime ExpireAt { get; private set; }

        public string Value { get; set; }

        public DateTime CreatedAt { get; set; }

        public void SetValue(string value)
        {
            Value = value;
        }

        public void SetExpireAt(DateTime expireAt)
        {
            ExpireAt = expireAt;
        }

        public void SetCreatedAt(DateTime createdAt)
        {
            CreatedAt = createdAt;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ExpireAt;
            yield return Value;
        }
    }
}
