namespace DevRating.ConsoleApp
{
    public sealed class ConsoleArguments : Arguments
    {
        private readonly string[] _args;

        public ConsoleArguments(string[] args)
        {
            _args = args;
        }

        public string Command()
        {
            return _args[0];
        }

        public string Path()
        {
            return ArgumentAt(1);
        }

        public string StartCommit()
        {
            return ArgumentAt(2);
        }

        public string EndCommit()
        {
            return ArgumentAt(3);
        }

        private string ArgumentAt(int i)
        {
            return _args.Length > i
                ? _args[i]
                : string.Empty;
        }
    }
}