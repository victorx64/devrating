using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface Works
    {
        InsertWorkOperation InsertOperation();

        Work Work(string repository, string start, string end);
        Work Work(object id);
        bool Contains(string repository, string start, string end);
        bool Contains(object id);
        IEnumerable<Work> LastWorks(string repository);
    }
}