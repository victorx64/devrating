namespace DevRating.Domain
{
    public interface Entity
    {
        object Id();
        string ToJson();
    }
}