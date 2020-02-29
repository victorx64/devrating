namespace DevRating.Domain
{
    public interface Deletion
    {
        string Email();
        uint Counted();
        uint Ignored();
    }
}