using System;

namespace DevRating.Domain
{
    public interface Rating : Entity
    {
        double Value();
        Rating PreviousRating();
        Envelope<IConvertible> Deletions();
        Work Work();
        Author Author();
    }
}