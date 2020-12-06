// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeRating : Rating
    {
        private readonly Id _id;
        private readonly double _value;
        private readonly Work _work;
        private readonly Author _author;
        private readonly Rating _previous;
        private readonly uint? _deletions;
        private readonly uint? _ignored;
        private readonly DateTimeOffset _createdAt;

        public FakeRating(double value, Work work, Author author)
            : this(
                value,
                work,
                author,
                new NullRating(),
                null,
                null,
                DateTimeOffset.UtcNow
            )
        {
        }

        public FakeRating(
            double value,
            Work work,
            Author author,
            Rating previous,
            uint? deletions,
            uint? ignored,
            DateTimeOffset createdAt
        )
            : this(new DefaultId(Guid.NewGuid()), value, work, author, previous, deletions, ignored, createdAt)
        {
        }

        public FakeRating(
            Id id,
            double value,
            Work work,
            Author author,
            Rating previous,
            uint? deletions,
            uint? ignored,
            DateTimeOffset createdAt
        )
        {
            _id = id;
            _value = value;
            _work = work;
            _author = author;
            _previous = previous;
            _deletions = deletions;
            _ignored = ignored;
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

        public double Value()
        {
            return _value;
        }

        public Rating PreviousRating()
        {
            return _previous;
        }

        public uint? CountedDeletions()
        {
            return _deletions;
        }

        public uint? IgnoredDeletions()
        {
            return _ignored;
        }

        public Work Work()
        {
            return _work;
        }

        public Author Author()
        {
            return _author;
        }

        public DateTimeOffset CreatedAt()
        {
            throw new NotImplementedException();
        }
    }
}