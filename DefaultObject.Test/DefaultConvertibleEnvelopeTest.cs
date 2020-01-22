using Xunit;

namespace DevRating.DefaultObject.Test
{
    public sealed class DefaultConvertibleEnvelopeTest
    {
        [Fact]
        public void IsNotFilledByDefault()
        {
            Assert.False(new DefaultConvertibleEnvelope().Filled());
        }
        
        [Fact]
        public void ReturnsDbNullValueByDefault()
        {
            Assert.Equal(System.DBNull.Value, new DefaultConvertibleEnvelope().Value());
        }
        
        [Fact]
        public void IsFilledWhenCreatedWithParam()
        {
            Assert.True(new DefaultConvertibleEnvelope("some data").Filled());
        }
        
        [Fact]
        public void ReturnsValueWhenCreatedWithParam()
        {
            Assert.Equal("some other data", new DefaultConvertibleEnvelope("some other data").Value());
        }
    }
}