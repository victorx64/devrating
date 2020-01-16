using System;
using Xunit;

namespace DevRating.VersionControl.Test
{
    public sealed class ContextLineEncounteredExceptionTest
    {
        [Fact]
        public void HasDefaultMessage()
        {
            Assert.False(string.IsNullOrEmpty(new ContextLineEncounteredException().Message));
        }

        [Fact]
        public void HasSetMessage()
        {
            Assert.Equal("Message", new ContextLineEncounteredException("Message").Message);
        }

        [Fact]
        public void HasSetInnerException()
        {
            Assert.NotNull(
                new ContextLineEncounteredException(
                        "Outer message",
                        new Exception("Inner exception")
                    )
                    .InnerException
            );
        }
    }
}