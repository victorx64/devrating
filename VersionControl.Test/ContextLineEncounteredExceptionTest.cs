// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text.Json;
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

        [Fact]
        public void CanBeSerialized()
        {
            Assert.Contains(
                "djh0g8973huhf",
                JsonSerializer.Serialize<ContextLineEncounteredException>(
                    new ContextLineEncounteredException("Message", new Exception("djh0g8973huhf"))
                )
            );
        }
    }
}