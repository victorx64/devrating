using DevRating.Git;
using DevRating.Rating;

namespace DevRating.Console
{
    public sealed class PlayersChangeLog : ChangeLog
    {
        private readonly Players _players;
        private readonly Player _default;
        private readonly PointsFormula _formula;

        public PlayersChangeLog(Players players, Player @default, PointsFormula formula)
        {
            _players = players;
            _default = @default;
            _formula = formula;
        }

        public void LogDeletion(string victim, string initiator, string commit)
        {
            if (victim.Equals(initiator))
            {
                return;
            }

            lock (_players)
            {
                var loser = PlayerOrDefault(victim);
                var winner = PlayerOrDefault(initiator);

                var extra = _formula.WinnerExtraPoints(winner.Points(), loser.Points());

                loser = loser.NewPlayer(new Game(initiator, commit, loser.Points() - extra, 0d, 1));
                AddOrUpdatePlayer(victim, loser);

                winner = winner.NewPlayer(new Game(victim, commit, winner.Points() + extra, 0d, 1));
                AddOrUpdatePlayer(initiator, winner);
            }
        }

        public void LogAddition(int count, string initiator, string commit)
        {
        }

        private Player PlayerOrDefault(string name)
        {
            if (_players.Exists(name))
            {
                return _players.Player(name);
            }

            return _default;
        }

        private void AddOrUpdatePlayer(string name, Player player)
        {
            if (_players.Exists(name))
            {
                _players.Update(name, player);
            }
            else
            {
                _players.Add(name, player);
            }
        }
    }
}