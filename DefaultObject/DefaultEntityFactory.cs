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
                _entities.Ratings().GetOperation().RatingOf(author.Id()).Id(), link);
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

            var winner = _entities.Ratings().GetOperation().RatingOf(author.Id());

            var value = winner.Id().Filled() ? winner.Value() : _formula.DefaultRating();

            var matches = MatchesWithInsertedLosers(deletions, work, value);

            _entities.Ratings().InsertOperation().Insert(
                _formula.WinnerNewRating(value, matches),
                new DefaultEnvelope(),
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

        private IEnumerable<Match> MatchesWithInsertedLosers(IEnumerable<Deletion> deletions, Id work, double winner)
        {
            var matches = new List<Match>();

            foreach (var deletion in deletions)
            {
                var victim = Author(deletion.Email());

                var current = _entities.Ratings().GetOperation().RatingOf(victim.Id());

                var value = current.Id().Filled() ? current.Value() : _formula.DefaultRating();

                _entities.Ratings().InsertOperation().Insert(
                    _formula.LoserNewRating(value, new DefaultMatch(winner, deletion.Count())),
                    new DefaultEnvelope(deletion.Count()),
                    current.Id(),
                    work,
                    victim.Id()
                );

                matches.Add(new DefaultMatch(value, deletion.Count()));
            }

            return matches;
        }

        private Author Author(string email)
        {
            return _entities.Authors().ContainsOperation().Contains(email)
                ? _entities.Authors().GetOperation().Author(email)
                : _entities.Authors().InsertOperation().Insert(email);
        }
    }
}