// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using DevRating.Domain;

namespace DevRating.DefaultObject
{
    public sealed class DefaultEnvelope : Envelope
    {
        private readonly IConvertible _value;

        public DefaultEnvelope() : this(DBNull.Value)
        {
        }

        public DefaultEnvelope(IConvertible value)
        {
            _value = value;
        }

        public bool Filled()
        {
            return !_value.Equals(DBNull.Value);
        }

        public IConvertible Value()
        {
            return _value;
        }
    }
}