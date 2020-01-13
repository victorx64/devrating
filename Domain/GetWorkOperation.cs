using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface GetWorkOperation
    {
        Work Work(string repository, string start, string end);
        Work Work(object id);
        IEnumerable<Work> Lasts(string repository);
    }
}