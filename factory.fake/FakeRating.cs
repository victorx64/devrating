using devrating.entity;

namespace devrating.factory.fake;

public sealed class FakeRating : Rating
{
    private readonly Id _id;
    private readonly double _value;
    private readonly Work _work;
    private readonly Author _author;
    private readonly Rating _previous;

    public FakeRating(double value, Work work, Author author)
        : this(
            value,
            work,
            author,
            new NullRating()
        )
    {
    }

    public FakeRating(
        double value,
        Work work,
        Author author,
        Rating previous
    )
        : this(new DefaultId(Guid.NewGuid()), value, work, author, previous)
    {
    }

    public FakeRating(
        Id id,
        double value,
        Work work,
        Author author,
        Rating previous
    )
    {
        _id = id;
        _value = value;
        _work = work;
        _author = author;
        _previous = previous;
    }

    public Id Id()
    {
        return _id;
    }

    public double Value()
    {
        return _value;
    }

    public Rating PreviousRating()
    {
        return _previous;
    }

    public Work Work()
    {
        return _work;
    }

    public Author Author()
    {
        return _author;
    }
}
