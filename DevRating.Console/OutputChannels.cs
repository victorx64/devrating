using DevRating.Rating;

namespace DevRating.Console
{
    public sealed class OutputChannels
    {
        private readonly string[] _arguments;
        private readonly IOutput _quite;
        private readonly IOutput _verbose;

        public OutputChannels(string[] arguments, IOutput quite, IOutput verbose)
        {
            _arguments = arguments;
            _quite = quite;
            _verbose = verbose;
        }

        public IOutput Channel()
        {
            return _verbose;
        }
    }
}