namespace DevRating.Domain
{
    public interface Rating : Entity
    {
        double Value();
        Rating PreviousRating();
        Envelope<uint> Deletions();
        Work Work();
        Author Author();
    }
}