namespace DevRating.Domain
{
    public interface Envelope<out T>
    {
        T Value();
        bool Filled();
    }
}