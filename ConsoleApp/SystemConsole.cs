namespace DevRating.ConsoleApp
{
    public sealed class SystemConsole : Console
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
}