using System.Collections.Generic;
using DevRating.DefaultObject;
using DevRating.Domain;

namespace DevRating.ConsoleApp.Fake
{
    public sealed class FakeDiff : Diff
    {
        private readonly string _key;
        private readonly string _start;
        private readonly string _end;
        private readonly string _email;
        private readonly string _organization;
        private readonly uint _additions;
        private readonly IEnumerable<Deletion> _deletions;

        public FakeDiff(string key, string start, string end, string email, string organization, uint additions,
            IEnumerable<Deletion> deletions)
        {
            _key = key;
            _start = start;
            _end = end;
            _email = email;
            _organization = organization;
            _additions = additions;
            _deletions = deletions;
        }

        public Work From(Works works)
        {
            return works.GetOperation().Work(_key, _start, _end);
        }

        public bool PresentIn(Works works)
        {
            return works.ContainsOperation().Contains(_key, _start, _end);
        }

        public void AddTo(EntityFactory factory)
        {
            factory.InsertRatings(
                _organization,
                _email,
                _deletions,
                factory.InsertedWork(_organization, _key, _start, _end, _email, _additions, new DefaultEnvelope()
                ).Id()
            );
        }
    }
}