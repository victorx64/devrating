using System.Collections.Generic;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeEntities : Entities
    {
        private readonly Works _works;
        private readonly Ratings _ratings;
        private readonly Authors _authors;

        public FakeEntities() : this(new List<Work>(), new List<Author>(), new List<Rating>())
        {
        }

        public FakeEntities(IList<Work> works, IList<Author> authors, IList<Rating> ratings)
            : this(
                new FakeWorks(works, authors, ratings),
                new FakeRatings(works, authors, ratings),
                new FakeAuthors(authors))
        {
        }

        public FakeEntities(Works works, Ratings ratings, Authors authors)
        {
            _works = works;
            _ratings = ratings;
            _authors = authors;
        }

        public Works Works()
        {
            return _works;
        }

        public Ratings Ratings()
        {
            return _ratings;
        }

        public Authors Authors()
        {
            return _authors;
        }
    }
}