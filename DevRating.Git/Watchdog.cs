namespace DevRating.Git
{
    public interface Watchdog
    {
        void WriteInto(Modifications modifications);
    }
}