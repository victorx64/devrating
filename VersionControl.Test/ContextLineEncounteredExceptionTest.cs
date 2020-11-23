// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
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
        public void CanBeSerializedAndDeserialized()
        {
            var source = new ContextLineEncounteredException("Message", new Exception("Inner exception."));

            var json = System.Text.Json.JsonSerializer.Serialize<ContextLineEncounteredException>(source);

            var dest= System.Text.Json.JsonSerializer.Deserialize<ContextLineEncounteredException>(json);

            Assert.Equal(source.ToString(), dest!.ToString());
        }
    }
}