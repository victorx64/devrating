using System;
using Xunit;

namespace DevRating.DefaultObject.Test
{
    public sealed class EmptyEnvelopeTest
    {
        [Fact]
        public void DoesntSupportValue()
        {
            object TestCode()
            {
                return new EmptyEnvelope<int>().Value();
            }

            Assert.Throws<NotSupportedException>(TestCode);
        }

        [Fact]
        public void IsNotFilled()
        {
            Assert.False(new EmptyEnvelope<int>().Filled());
        }
    }
}