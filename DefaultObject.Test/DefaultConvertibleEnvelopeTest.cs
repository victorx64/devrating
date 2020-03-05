// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace DevRating.DefaultObject.Test
{
    public sealed class DefaultConvertibleEnvelopeTest
    {
        [Fact]
        public void IsNotFilledByDefault()
        {
            Assert.False(new DefaultEnvelope().Filled());
        }
        
        [Fact]
        public void ReturnsDbNullValueByDefault()
        {
            Assert.Equal(System.DBNull.Value, new DefaultEnvelope().Value());
        }
        
        [Fact]
        public void IsFilledWhenCreatedWithParam()
        {
            Assert.True(new DefaultEnvelope("some data").Filled());
        }
        
        [Fact]
        public void ReturnsValueWhenCreatedWithParam()
        {
            Assert.Equal("some other data", new DefaultEnvelope("some other data").Value());
        }
    }
}