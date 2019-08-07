using DevRating.Rating;

namespace DevRating.Console
{
    public sealed class OutputChannels
    {
        private readonly string[] _arguments;
        private readonly Output _quite;
        private readonly Output _verbose;

        public OutputChannels(string[] arguments, Output quite, Output verbose)
        {
            _arguments = arguments;
            _quite = quite;
            _verbose = verbose;
        }

        public Output Channel()
        {
            return _verbose;
        }
    }
}