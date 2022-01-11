using System.Text.Json;
using Xunit;

namespace devrating.git.test;

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
