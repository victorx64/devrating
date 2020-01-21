namespace DevRating.Domain
{
    public interface Id : Envelope
    {
        bool Present();
    }
}