using System;
using System.Collections.Generic;
using System.Linq;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeInsertWorkOperation : InsertWorkOperation
    {
        private readonly IList<Work> _works;

        public FakeInsertWorkOperation(IList<Work> works)
        {
            _works = works;
        }

        public Work Insert(string repository, string start, string end, Entity author, uint additions, Entity rating,
            ObjectEnvelope link)
        {
            var work = new FakeWork(
                Guid.NewGuid(),
                additions,
                (Author) author,
                (Rating) rating
            );

            _works.Add(work);

            return work;
        }
    }
}