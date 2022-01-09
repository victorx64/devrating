namespace devrating.entity;

public interface Rating : Entity
{
    double Value();
    Rating PreviousRating();
    Work Work();
    Author Author();
}
