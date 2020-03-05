// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace DevRating.DefaultObject.Test
{
    public sealed class DefaultMatchTest
    {
        [Fact]
        public void ReturnsContenderRatingFromCtor()
        {
            var rating = 1d;

            Assert.Equal(rating, new DefaultMatch(rating, 1).ContenderRating());
        }

        [Fact]
        public void ReturnsCountFromCtor()
        {
            var count = 2u;

            Assert.Equal(count, new DefaultMatch(2d, count).Count());
        }
    }
}