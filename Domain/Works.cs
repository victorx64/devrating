namespace DevRating.Domain
{
    public interface Works
    {
        InsertWorkOperation InsertOperation();
        GetWorkOperation GetOperation();
        bool Contains(string repository, string start, string end);
        bool Contains(object id);
    }
}