// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using DevRating.Domain;

namespace DevRating.DefaultObject
{
    public sealed class DefaultId : Id
    {
        private readonly object _value;

        public DefaultId() : this(DBNull.Value)
        {
        }

        public DefaultId(object value)
        {
            _value = value;
        }

        public bool Filled()
        {
            return !_value.Equals(DBNull.Value);
        }

        public object Value()
        {
            return _value;
        }

        public bool Equals(Id other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _value.Equals(other.Value());
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is DefaultId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(DefaultId? left, DefaultId? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DefaultId? left, DefaultId? right)
        {
            return !Equals(left, right);
        }
    }
}