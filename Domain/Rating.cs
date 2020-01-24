namespace DevRating.Domain
{
    public interface Rating : Entity
    {
        double Value();
        Rating PreviousRating();
        Envelope Deletions();
        Work Work();
        Author Author();
    }
}