namespace DevRating.Console
{
    public class DefaultArguments : Arguments
    {
        private readonly string[] _arguments;

        public DefaultArguments(string[] arguments)
        {
            _arguments = arguments;
        }
        
        public string OldestCommit()
        {
            if (_arguments.Length > 0)
            {
                return _arguments[0];
            }

            return string.Empty;
        }
        
        public string NewestCommit()
        {
            if (_arguments.Length > 1)
            {
                return _arguments[1];
            }

            return string.Empty;
        }
    }
}