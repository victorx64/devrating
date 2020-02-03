using System.Collections.Generic;

namespace DevRating.ConsoleApp.Fake
{
    public sealed class FakeConsole : Console
    {
        private readonly IList<string> _lines;

        public FakeConsole(IList<string> lines)
        {
            _lines = lines;
        }

        public void WriteLine()
        {
            _lines.Add(string.Empty);
        }

        public void WriteLine(string value)
        {
            _lines.Add(value);
        }
    }
}