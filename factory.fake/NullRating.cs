using devrating.entity;

namespace devrating.factory.fake;

public sealed class NullRating : Rating
{
    public Id Id()
    {
        return new DefaultId();
    }

    public double Value()
    {
        throw new NotSupportedException();
    }

    public Rating PreviousRating()
    {
        throw new NotSupportedException();
    }

    public Work Work()
    {
        throw new NotSupportedException();
    }

    public Author Author()
    {
        throw new NotSupportedException();
    }
}
