using DevRating.Rating;

namespace DevRating.VersionControlSystem.Git
{
    public class Modification
    {
        private readonly string _previous;
        
        private readonly string _next;

        public Modification(string previous, string next)
        {
            _previous = previous;
            
            _next = next;
        }

        public IRating UpdatedRating(IRating rating)
        {
            return rating.UpdatedRating(_previous, _next);
        }
    }
}