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

        public FakeAuthor(string organization, string email) : this(new DefaultId(Guid.NewGuid()), email, organization)
        {
        }

        public FakeAuthor(Id id, string email, string organization)
        {
            _id = id;
            _email = email;
            _organization = organization;
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
            throw new NotImplementedException();
        }
    }
}