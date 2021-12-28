// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DevRating.Domain;

namespace DevRating.DefaultObject
{
    public sealed class DefaultMatch : Match
    {
        private readonly double _rating;
        private readonly uint _count;

        public DefaultMatch(double rating, uint count)
        {
            _rating = rating;
            _count = count;
        }

        public double ContenderRating()
        {
            return _rating;
        }

        public uint Count()
        {
            return _count;
        }
    }
}