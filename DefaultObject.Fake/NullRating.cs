// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class NullRating : Rating
    {
        private readonly double _value;

        public NullRating() : this(0d)
        {
        }

        public NullRating(double value)
        {
            _value = value;
        }

        public Id Id()
        {
            return new DefaultId();
        }

        public string ToJson()
        {
            throw new NotSupportedException();
        }

        public double Value()
        {
            return _value;
        }

        public Rating PreviousRating()
        {
            throw new NotSupportedException();
        }

        public uint? CountedDeletions()
        {
            throw new NotSupportedException();
        }

        public uint? IgnoredDeletions()
        {
            throw new NotSupportedException();
        }

        public Work Work()
        {
            throw new NotSupportedException();
        }

        public Author Author()
        {
            throw new NotSupportedException();
        }

        public DateTimeOffset CreatedAt()
        {
            throw new NotImplementedException();
        }
    }
}