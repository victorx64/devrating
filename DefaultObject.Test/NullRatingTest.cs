using System;
using Xunit;

namespace DevRating.DefaultObject.Test
{
    public sealed class NullRatingTest
    {
        [Fact]
        public void ReturnsNullId()
        {
            Assert.Equal(DBNull.Value, new NullRating().Id().Value());
        }

        [Fact]
        public void ReturnsDefaultValue()
        {
            Assert.Equal(24124d, new NullRating(24124d).Value());
        }

        [Fact]
        public void DoesntSupportToJson()
        {
            Assert.Throws<NotSupportedException>(new NullRating().ToJson);
        }

        [Fact]
        public void DoesntSupportPreviousRating()
        {
            Assert.Throws<NotSupportedException>(new NullRating().PreviousRating);
        }

        [Fact]
        public void DoesntSupportDeletions()
        {
            object TestCode()
            {
                return new NullRating().Deletions();
            }

            Assert.Throws<NotSupportedException>(TestCode);
        }

        [Fact]
        public void DoesntSupportWork()
        {
            Assert.Throws<NotSupportedException>(new NullRating().Work);
        }

        [Fact]
        public void DoesntSupportAuthor()
        {
            Assert.Throws<NotSupportedException>(new NullRating().Author);
        }
    }
}