using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface Works
    {
        Work Insert(
            string repository,
            string start,
            string end,
            Entity author,
            uint additions,
            Entity rating);

        Work Insert(
            string repository,
            string start,
            string end,
            Entity author,
            uint additions);

        Work Insert(
            string repository,
            string start,
            string end,
            Entity author,
            uint additions,
            Entity rating,
            string link);

        Work Insert(
            string repository,
            string start,
            string end,
            Entity author,
            uint additions,
            string link);

        Work Work(string repository, string start, string end);
        Work Work(object id);
        bool Contains(string repository, string start, string end);
        bool Contains(object id);
        IEnumerable<Work> LastWorks(string repository);
    }
}