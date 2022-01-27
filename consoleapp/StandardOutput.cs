namespace devrating.consoleapp;

public sealed class StandardOutput : Output
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
