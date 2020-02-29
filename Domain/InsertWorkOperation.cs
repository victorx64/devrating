namespace DevRating.Domain
{
    public interface InsertWorkOperation
    {
        Work Insert(
            string repository,
            string start,
            string end,
            Envelope since,
            Id author,
            uint additions,
            Id rating,
            Envelope link
        );
    }
}