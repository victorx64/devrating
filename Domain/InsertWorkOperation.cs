namespace DevRating.Domain
{
    public interface InsertWorkOperation
    {
        Work Insert(string repository, string start, string end, Entity author, uint additions,
            Entity rating, DbParameter link);
    }
}