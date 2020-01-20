using Xunit;

namespace DevRating.DefaultObject.Test
{
    public sealed class DefaultObjectEnvelopeTest
    {
        [Fact]
        public void ReturnsValueFromCtor()
        {
            var rating = 1d;

            Assert.Equal(rating, new DefaultObjectEnvelope(rating).Value());
        }
    }
}