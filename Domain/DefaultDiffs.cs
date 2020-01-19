using System;
using System.Collections.Generic;
using System.Linq;

namespace DevRating.Domain
{
    public sealed class DefaultDiffs : InsertWorkParams
    {
        private readonly string _repository;
        private readonly string _start;
        private readonly string _end;
        private readonly uint _additions;
        private readonly string _email;
        private readonly Deletions _deletions;

        public DefaultDiffs(string repository, string start, string end, uint additions, string email,
            Deletions deletions)
        {
            _repository = repository;
            _start = start;
            _end = end;
            _additions = additions;
            _email = email;
            _deletions = deletions;
        }

        public void InsertionResult(Entities entities, Formula formula)
        {
            var author = Author(entities, _email);

            InsertNewRatings(entities, formula, author, InsertedWork(entities, author));
        }

        private void InsertNewRatings(Entities entities, Formula formula, Entity author, Entity work)
        {
            var victims = Victims();

            if (victims.Any())
            {
                InsertAuthorNewRating(entities, formula, author, victims, work);

                InsertVictimsNewRatings(entities, formula, victims, work, RatingOf(entities, formula, author));
            }
        }

        private IList<Deletion> Victims()
        {
            bool NotSelfDeletion(Deletion deletion)
            {
                return !deletion.Email().Equals(_email, StringComparison.OrdinalIgnoreCase);
            }

            return _deletions.Items().Where(NotSelfDeletion).ToList();
        }

        private Author Author(Entities entities, string email)
        {
            return entities.Authors().ContainsOperation().Contains(email)
                ? entities.Authors().GetOperation().Author(email)
                : entities.Authors().InsertOperation().Insert(email);
        }

        private Work InsertedWork(Entities entities, Entity author)
        {
            if (entities.Ratings().ContainsOperation().ContainsRatingOf(author))
            {
                return entities.Works().InsertOperation().Insert(_repository, _start, _end, author, _additions,
                    entities.Ratings().GetOperation().RatingOf(author), new NullDbParameter());
            }
            else
            {
                return entities.Works().InsertOperation()
                    .Insert(_repository, _start, _end, author, _additions, new NullDbEntity(), new NullDbParameter());
            }
        }

        private double RatingOf(Entities entities, Formula formula, Entity author)
        {
            return entities.Ratings().ContainsOperation().ContainsRatingOf(author)
                ? entities.Ratings().GetOperation().RatingOf(author).Value()
                : formula.DefaultRating();
        }

        private void InsertAuthorNewRating(Entities entities, Formula formula, Entity author,
            IEnumerable<Deletion> victims, Entity work)
        {
            var @new = formula.WinnerNewRating(RatingOf(entities, formula, author),
                victims.Select(deletion =>
                    new DefaultMatch(RatingOf(entities, formula, Author(entities, deletion.Email())),
                        deletion.Count())));

            if (entities.Ratings().ContainsOperation().ContainsRatingOf(author))
            {
                entities.Ratings().InsertOperation()
                    .Insert(@new, new NullDbParameter(), entities.Ratings().GetOperation().RatingOf(author), work,
                        author);
            }
            else
            {
                entities.Ratings().InsertOperation()
                    .Insert(@new, new NullDbParameter(), new NullDbEntity(), work, author);
            }
        }

        private void InsertVictimsNewRatings(Entities entities, Formula formula, IEnumerable<Deletion> victims,
            Entity work, double rating)
        {
            foreach (var deletion in victims)
            {
                var victim = Author(entities, deletion.Email());

                var current = RatingOf(entities, formula, victim);

                var @new = formula.LoserNewRating(current, new DefaultMatch(rating, deletion.Count()));

                if (entities.Ratings().ContainsOperation().ContainsRatingOf(victim))
                {
                    entities.Ratings().InsertOperation()
                        .Insert(@new, new NullDbParameter(), entities.Ratings().GetOperation().RatingOf(victim), work,
                            victim);
                }
                else
                {
                    entities.Ratings().InsertOperation()
                        .Insert(@new, new NullDbParameter(), new NullDbEntity(), work, victim);
                }
            }
        }
    }
}