using DevRating.Git;
using DevRating.Rating;

namespace DevRating.Game
{
    public sealed class GamesFactory : ModificationsFactory
    {
        private readonly Formula _formula;
        private readonly double _threshold;

        public GamesFactory(Formula formula, double threshold)
        {
            _formula = formula;
            _threshold = threshold;
        }
        
        public Modifications Modifications(string commit, string author)
        {
            return new Games(commit, author, _formula, _threshold);
        }
    }
}