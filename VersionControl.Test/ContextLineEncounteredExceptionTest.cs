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
            Exception ex = new ContextLineEncounteredException("Message", new Exception("Inner exception."));

            var exceptionToString = ex.ToString();
            var bf = new BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, ex);

                ms.Seek(0, 0);

                ex = (ContextLineEncounteredException) bf.Deserialize(ms);
            }

            Assert.Equal(exceptionToString, ex.ToString());
        }
    }
}