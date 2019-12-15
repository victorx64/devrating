namespace DevRating.Domain
{
    public interface Works
    {
        Work Insert(
            string repository,
            string start,
            string end,
            IdObject author,
            uint additions,
            IdObject rating);

        Work Insert(
            string repository,
            string start,
            string end,
            IdObject author,
            uint additions);

        Work Work(string repository, string start, string end);
        bool Contains(string repository, string start, string end);
    }
}