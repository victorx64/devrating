namespace DevRating.Domain
{
    public interface InsertWorkOperation
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
    }
}