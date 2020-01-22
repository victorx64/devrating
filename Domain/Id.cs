namespace DevRating.Domain
{
    public interface Id
    {
        object Value();
        bool Filled();
    }
}