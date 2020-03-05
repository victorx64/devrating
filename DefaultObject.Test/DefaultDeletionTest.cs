// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace DevRating.DefaultObject.Test
{
    public sealed class DefaultDeletionTest
    {
        [Fact]
        public void ReturnsEmailFromCtor()
        {
            var email = "some email";

            Assert.Equal(email, new DefaultDeletion(email, 1).Email());
        }

        [Fact]
        public void ReturnsCountFromCtor()
        {
            var count = 2u;

            Assert.Equal(count, new DefaultDeletion("some other email", count).Count());
        }
    }
}