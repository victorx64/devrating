namespace DevRating.Domain
{
    public interface Rating : Entity
    {
        double Value();
        Rating PreviousRating();
        Envelope CountedDeletions();
        Envelope IgnoredDeletions();
        Work Work();
        Author Author();
    }
}