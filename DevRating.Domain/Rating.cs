namespace DevRating.Domain
{
    public interface Rating
    {
        Author Author();
        double Value();
    }
}