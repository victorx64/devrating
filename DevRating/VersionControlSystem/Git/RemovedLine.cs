using DevRating.Rating;

namespace DevRating.VersionControlSystem.Git
{
    public class RemovedLine
    {
        private readonly string _previousAuthorEmail;
        private readonly string _nextAuthorEmail;

        public RemovedLine(string previousAuthorEmail, string nextAuthorEmail)
        {
            _previousAuthorEmail = previousAuthorEmail;
            _nextAuthorEmail = nextAuthorEmail;
        }

        public IRating UpdatedRating(IRating rating)
        {
            return rating.Update(_previousAuthorEmail, _nextAuthorEmail);
        }
    }
}