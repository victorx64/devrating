using devrating.factory;

namespace devrating.consoleapp;

public sealed class StandardOutput : Log
{
    public void WriteLine()
    {
        System.Console.WriteLine();
    }

    public void WriteLine(string value)
    {
        System.Console.WriteLine(value);
    }
}
