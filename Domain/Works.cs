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

        Work Work(string repository, string start, string end);
        Work Work(object id);
        bool Contains(string repository, string start, string end);
        bool Contains(object id);
    }
}