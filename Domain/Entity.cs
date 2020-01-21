namespace DevRating.Domain
{
    public interface Entity
    {
        Id Id();
        string ToJson();
    }
}