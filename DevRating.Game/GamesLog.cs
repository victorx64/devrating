using DevRating.Git;
using DevRating.Rating;

namespace DevRating.Game
{
    public sealed class GamesLog : Log
    {
        private readonly Players _players;
        private readonly Formula _formula;
        private readonly double _threshold;

        public GamesLog(Players players, Formula formula, double threshold)
        {
            _players = players;
            _formula = formula;
            _threshold = threshold;
        }

        public void LogDeletion(int count, string victim, string initiator, string commit)
        {
            lock (_players)
            {
                var loser = _players.PlayerOrDefault(victim);
                var winner = _players.PlayerOrDefault(initiator);

                // multiplying to 'count' is gross simplification
                var extra = _formula.WinnerExtraPoints(winner.Points(), loser.Points()) * count;
                var reward = _formula.WinProbability(winner.Points(), loser.Points()) * count;

                _players.AddOrUpdatePlayer(victim,
                    loser.PerformedPlayer(initiator, commit, loser.Points() - extra, 0d, count));
                _players.AddOrUpdatePlayer(initiator,
                    winner.PerformedPlayer(victim, commit, winner.Points() + extra, reward, count));
            }
        }

        public void LogAddition(int count, string initiator, string commit)
        {
            lock (_players)
            {
                var winner = _players.PlayerOrDefault(initiator);

                var reward = _formula.WinProbability(winner.Points(), _threshold) * count;

                _players.AddOrUpdatePlayer(initiator,
                    winner.PerformedPlayer("emptiness", commit, winner.Points(), reward, count)); // TODO "emptiness" can collide
            }
        }
    }
}