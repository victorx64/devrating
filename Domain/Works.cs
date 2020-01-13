namespace DevRating.Domain
{
    public interface Works
    {
        InsertWorkOperation InsertOperation();
        GetWorkOperation GetOperation();
        ContainsWorkOperation ContainsOperation();
    }
}