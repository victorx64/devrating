using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface GetWorkOperation
    {
        Work Work(string repository, string start, string end);
        Work Work(Id id);
        IEnumerable<Work> Lasts(string repository);
    }
}