using System;
using System.Collections.Generic;
using System.Linq;

namespace DevRating.Domain
{
    public sealed class DefaultDiffs : Diffs
    {
        private readonly Database _database;
        private readonly Formula _formula;

        public DefaultDiffs(Database database, Formula formula)
        {
            _database = database;
            _formula = formula;
        }

        public Database Database()
        {
            return _database;
        }

        public Formula Formula()
        {
            return _formula;
        }

        public void Insert(string repository, string start, string end, string email, uint additions,
            IEnumerable<Deletion> deletions)
        {
            var author = Author(email);

            InsertNewRatings(email, deletions, author, InsertedWork(repository, start, end, additions, author));
        }

        public void Insert(string repository, string link, string start, string end, string email, uint additions,
            IEnumerable<Deletion> deletions)
        {
            var author = Author(email);

            InsertNewRatings(email, deletions, author, InsertedWork(repository, link, start, end, additions, author));
        }

        private void InsertNewRatings(string email, IEnumerable<Deletion> deletions, Entity author, Entity work)
        {
            var victims = Victims(email, deletions);

            if (victims.Any())
            {
                InsertAuthorNewRating(author, victims, work);

                InsertVictimsNewRatings(victims, work, RatingOf(author));
            }
        }

        private IList<Deletion> Victims(string email, IEnumerable<Deletion> deletions)
        {
            bool NotSelfDeletion(Deletion deletion)
            {
                return !deletion.Email().Equals(email, StringComparison.OrdinalIgnoreCase);
            }

            return deletions.Where(NotSelfDeletion).ToList();
        }

        private Author Author(string email)
        {
            return _database.Entities().Authors().Contains(email)
                ? _database.Entities().Authors().Author(email)
                : _database.Entities().Authors().Insert(email);
        }

        private Work InsertedWork(string repository, string start, string end, uint additions, Entity author)
        {
            if (_database.Entities().Ratings().ContainsRatingOf(author))
            {
                return _database.Entities().Works().InsertOperation().Insert(repository, start, end, author, additions,
                    _database.Entities().Ratings().GetOperation().RatingOf(author));
            }
            else
            {
                return _database.Entities().Works().InsertOperation().Insert(repository, start, end, author, additions);
            }
        }

        private Work InsertedWork(string repository, string link, string start, string end, uint additions,
            Entity author)
        {
            if (_database.Entities().Ratings().ContainsRatingOf(author))
            {
                return _database.Entities().Works().InsertOperation().Insert(repository, start, end, author, additions,
                    _database.Entities().Ratings().GetOperation().RatingOf(author), link);
            }
            else
            {
                return _database.Entities().Works().InsertOperation()
                    .Insert(repository, start, end, author, additions, link);
            }
        }

        private double RatingOf(Entity author)
        {
            return _database.Entities().Ratings().ContainsRatingOf(author)
                ? _database.Entities().Ratings().GetOperation().RatingOf(author).Value()
                : _formula.DefaultRating();
        }

        private void InsertAuthorNewRating(Entity author, IEnumerable<Deletion> victims, Entity work)
        {
            var @new = _formula.WinnerNewRating(RatingOf(author), victims.Select(Match));

            if (_database.Entities().Ratings().ContainsRatingOf(author))
            {
                _database.Entities().Ratings().InsertOperation()
                    .Insert(author, @new, _database.Entities().Ratings().GetOperation().RatingOf(author), work);
            }
            else
            {
                _database.Entities().Ratings().InsertOperation().Insert(author, @new, work);
            }
        }

        private void InsertVictimsNewRatings(IEnumerable<Deletion> victims, Entity work, double rating)
        {
            foreach (var deletion in victims)
            {
                var victim = Author(deletion.Email());

                var current = RatingOf(victim);

                var @new = _formula.LoserNewRating(current, new DefaultMatch(rating, deletion.Count()));

                if (_database.Entities().Ratings().ContainsRatingOf(victim))
                {
                    _database.Entities().Ratings().InsertOperation()
                        .Insert(victim, @new, _database.Entities().Ratings().GetOperation().RatingOf(victim), work);
                }
                else
                {
                    _database.Entities().Ratings().InsertOperation().Insert(victim, @new, work);
                }
            }
        }

        private Match Match(Deletion deletion)
        {
            return new DefaultMatch(RatingOf(Author(deletion.Email())), deletion.Count());
        }
    }
}