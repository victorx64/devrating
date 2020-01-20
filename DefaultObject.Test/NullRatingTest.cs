using System;
using Xunit;

namespace DevRating.DefaultObject.Test
{
    public sealed class NullRatingTest
    {
        [Fact]
        public void ReturnsNullId()
        {
            Assert.Equal(DBNull.Value, new NullRating().Id());
        }

        [Fact]
        public void ReturnsDefaultValue()
        {
            Assert.Equal(24124d, new NullRating(24124d).Value());
        }

        [Fact]
        public void DoesntImplementToJson()
        {
            Assert.Throws<NotImplementedException>(new NullRating().ToJson);
        }

        [Fact]
        public void DoesntImplementHasPreviousRating()
        {
            object TestCode()
            {
                return new NullRating().HasPreviousRating();
            }

            Assert.Throws<NotImplementedException>(TestCode);
        }

        [Fact]
        public void DoesntImplementPreviousRating()
        {
            Assert.Throws<NotImplementedException>(new NullRating().PreviousRating);
        }

        [Fact]
        public void DoesntImplementHasDeletions()
        {
            object TestCode()
            {
                return new NullRating().HasDeletions();
            }

            Assert.Throws<NotImplementedException>(TestCode);
        }

        [Fact]
        public void DoesntImplementDeletions()
        {
            object TestCode()
            {
                return new NullRating().Deletions();
            }

            Assert.Throws<NotImplementedException>(TestCode);
        }

        [Fact]
        public void DoesntImplementWork()
        {
            Assert.Throws<NotImplementedException>(new NullRating().Work);
        }

        [Fact]
        public void DoesntImplementAuthor()
        {
            Assert.Throws<NotImplementedException>(new NullRating().Author);
        }
    }
}