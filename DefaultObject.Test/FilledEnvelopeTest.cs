using Xunit;

namespace DevRating.DefaultObject.Test
{
    public sealed class FilledEnvelopeTest
    {
        [Fact]
        public void ReturnsValueFromCtor()
        {
            Assert.Equal(123123, new FilledEnvelope<int>(123123).Value());
        }

        [Fact]
        public void IsFilled()
        {
            Assert.True(new FilledEnvelope<int>(1231).Filled());
        }
    }
}