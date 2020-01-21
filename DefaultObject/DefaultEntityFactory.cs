using System;
using System.Collections.Generic;
using System.Linq;
using DevRating.Domain;

namespace DevRating.DefaultObject
{
    public sealed class DefaultEntityFactory : EntityFactory
    {
        private readonly Entities _entities;
        private readonly Formula _formula;

        public DefaultEntityFactory(Entities entities, Formula formula)
        {
            _entities = entities;
            _formula = formula;
        }

        public Work InsertedWork(string repository, string start, string end, string email, uint additions,
            Envelope link)
        {
            var author = Author(email);

            return _entities.Works().InsertOperation().Insert(repository, start, end, author.Id(), additions,
                RatingOf(author.Id()).Id(), link);
        }

        public void InsertRatings(string email, IEnumerable<Deletion> deletions, Id work)
        {
            var items = NonSelfDeletions(email, deletions);

            if (!items.Any())
            {
                return;
            }

            InsertRatings(email, work, items);
        }

        private void InsertRatings(string email, Id work, IEnumerable<Deletion> deletions)
        {
            var author = Author(email);

            var winner = RatingOf(author.Id());

            var matches = MatchesWithInsertedLosers(deletions, work, winner);

            _entities.Ratings().InsertOperation().Insert(
                _formula.WinnerNewRating(winner.Value(), matches),
                new EmptyEnvelope(),
                winner.Id(),
                work,
                author.Id()
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

        private IEnumerable<Match> MatchesWithInsertedLosers(IEnumerable<Deletion> deletions, Id work, Rating winner)
        {
            var matches = new List<Match>();

            foreach (var deletion in deletions)
            {
                var victim = Author(deletion.Email());

                var current = RatingOf(victim.Id());

                _entities.Ratings().InsertOperation().Insert(
                    _formula.LoserNewRating(
                        current.Value(),
                        new DefaultMatch(winner.Value(), deletion.Count())
                    ),
                    new DefaultId(deletion.Count()),
                    current.Id(),
                    work,
                    victim.Id()
                );

                matches.Add(new DefaultMatch(current.Value(), deletion.Count()));
            }

            return matches;
        }

        private Rating RatingOf(Id author)
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