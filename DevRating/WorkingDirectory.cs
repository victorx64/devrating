namespace DevRating
{
    public class WorkingDirectory : IWorkingDirectory
    {
        private readonly string[] _arguments;

        public WorkingDirectory(string[] arguments)
        {
            _arguments = arguments;
        }

        public override string ToString()
        {
            return _arguments.Length > 0
                ? _arguments[0]
                : string.Empty;
        }
    }
}