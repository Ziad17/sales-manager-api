using System;
using System.Collections.Generic;
using Vizage.Infrastructure.Domain.Entities;
using Vizage.Infrastructure.Domain.Guards;
using Vizage.Infrastructure.Domain.Guards.Abstractions;

namespace Vizage.Modules.Users.Domain.ValueObjects
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
            Guard.Check.IfNullOrEmpty(value);
            Value = value;
        }

        public void SetExpireAt(DateTime expireAt)
        {
            Guard.Check.IfDefault(expireAt);
            ExpireAt = expireAt;
        }

        public void SetCreatedAt(DateTime createdAt)
        {
            Guard.Check.IfDefault(createdAt);
            CreatedAt = createdAt;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ExpireAt;
            yield return Value;
        }
    }
}
