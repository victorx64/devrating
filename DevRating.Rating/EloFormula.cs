using System;

namespace DevRating.Rating
{
    public class EloFormula : Formula
    {
        private readonly double _k;
        private readonly double _n;
        private readonly double _default;
        private readonly double _boss;

        public EloFormula() : this(2d, 400d, 1200d, 2000d)
        {
        }

        public EloFormula(double k, double n, double @default, double boss)
        {
            _k = k;
            _n = n;
            _default = @default;
            _boss = boss;
        }

        public double BossRating()
        {
            return _boss;
        }

        public double NewPlayerRating()
        {
            return _default;
        }

        public double WinnerNewRating(Match match)
        {
            return match.Winner() + WinnerExtraPoints(match.Winner(), match.Loser()) * match.Count();
        }

        public double LoserNewRating(Match match)
        {
            return match.Loser() - WinnerExtraPoints(match.Winner(), match.Loser()) * match.Count();
        }

        public double WinnerReward(Match match)
        {
            return WinProbability(match.Winner(), match.Loser()) * match.Count();
        }

        public double LoserReward(Match match)
        {
            return 0d;
        }

        private double WinnerExtraPoints(double winner, double loser)
        {
            return _k * (1d - WinProbability(winner, loser));
        }

        private double WinProbability(double winner, double loser)
        {
            var qa = Math.Pow(10d, winner / _n);

            var qb = Math.Pow(10d, loser / _n);

            var ea = qa / (qa + qb);

            return ea;
        }
    }
}