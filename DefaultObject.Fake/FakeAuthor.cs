// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeAuthor : Author
    {
        private readonly Id _id;
        private readonly string _email;
        private readonly string _organization;
        private readonly DateTimeOffset _createdAt;

        public FakeAuthor(string organization, string email)
            : this(organization, email, DateTimeOffset.UtcNow)
        {
        }

        public FakeAuthor(string organization, string email, DateTimeOffset createdAt)
            : this(new DefaultId(Guid.NewGuid()), email, organization, createdAt)
        {
        }

        public FakeAuthor(Id id, string email, string organization, DateTimeOffset createdAt)
        {
            _id = id;
            _email = email;
            _organization = organization;
            _createdAt = createdAt;
        }

        public Id Id()
        {
            return _id;
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }

        public string Email()
        {
            return _email;
        }

        public string Organization()
        {
            return _organization;
        }

        public DateTimeOffset CreatedAt()
        {
            return _createdAt;
        }
    }
}