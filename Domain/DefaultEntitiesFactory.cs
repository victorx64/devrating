using System;
using System.Collections.Generic;
using System.Linq;

namespace DevRating.Domain
{
    public sealed class DefaultEntitiesFactory : EntitiesFactory
    {
        private readonly Entities _entities;
        private readonly Formula _formula;

        public DefaultEntitiesFactory(Entities entities, Formula formula)
        {
            _entities = entities;
            _formula = formula;
        }

        public Work InsertedWork(string repository, string start, string end, string email, uint additions)
        {
            var author = Author(email);

            return _entities.Works().InsertOperation().Insert(repository, start, end, author, additions,
                RatingOf(author), new NullDbParameter());
        }

        public void InsertRatings(string email, IEnumerable<Deletion> deletions, Entity work)
        {
            var items = NonSelfDeletions(email, deletions);

            if (!items.Any())
            {
                return;
            }

            InsertRatings(email, work, items);
        }

        private void InsertRatings(string email, Entity work, IEnumerable<Deletion> deletions)
        {
            var author = Author(email);

            var winner = RatingOf(author);

            var matches = MatchesWithInsertedLosers(deletions, work, winner);

            _entities.Ratings().InsertOperation().Insert(
                _formula.WinnerNewRating(winner.Value(), matches),
                new NullDbParameter(),
                winner,
                work,
                author
            );
        }

        private IList<Deletion> NonSelfDeletions(string email, IEnumerable<Deletion> deletions)
        {
            bool NonSelfDeletion(Deletion d)
            {
                return !d.Email().Equals(email, StringComparison.OrdinalIgnoreCase);
            }

            return deletions.Where(NonSelfDeletion).ToList();
        }

        private IEnumerable<Match> MatchesWithInsertedLosers(IEnumerable<Deletion> deletions, Entity work,
            Rating winner)
        {
            var matches = new List<Match>();

            foreach (var deletion in deletions)
            {
                var victim = Author(deletion.Email());

                var current = RatingOf(victim);

                _entities.Ratings().InsertOperation().Insert(
                    _formula.LoserNewRating(
                        current.Value(),
                        new DefaultMatch(winner.Value(), deletion.Count())
                    ),
                    new DefaultDbParameter(deletion.Count()),
                    current,
                    work,
                    victim
                );

                matches.Add(new DefaultMatch(current.Value(), deletion.Count()));
            }

            return matches;
        }

        private Rating RatingOf(Entity author)
        {
            return _entities.Ratings().ContainsOperation().ContainsRatingOf(author)
                ? _entities.Ratings().GetOperation().RatingOf(author)
                : new NullRating(_formula.DefaultRating());
        }

        private Author Author(string email)
        {
            return _entities.Authors().ContainsOperation().Contains(email)
                ? _entities.Authors().GetOperation().Author(email)
                : _entities.Authors().InsertOperation().Insert(email);
        }
    }
}