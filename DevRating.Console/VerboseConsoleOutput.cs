using DevRating.Rating;

namespace DevRating.Console
{
    public sealed class VerboseConsoleOutput : Output
    {
        public void Write(string line)
        {
            System.Console.Write(line);
        }

        public void WriteLine(string line)
        {
            System.Console.WriteLine(line);
        }

        public void WriteVerbose(string line)
        {
            System.Console.Write(line);
        }

        public void WriteVerboseLine(string line)
        {
            System.Console.WriteLine(line);
        }
    }
}