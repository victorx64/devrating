namespace devrating.factory;

public sealed class DefaultFactories : Factories
{
    private readonly AuthorFactory _authorFactory;
    private readonly WorkFactory _workFactory;
    private readonly RatingFactory _ratingFactory;

    public DefaultFactories(
        AuthorFactory authorFactory,
        WorkFactory workFactory,
        RatingFactory ratingFactory
    )
    {
        _authorFactory = authorFactory;
        _workFactory = workFactory;
        _ratingFactory = ratingFactory;
    }

    public AuthorFactory AuthorFactory()
    {
        return _authorFactory;
    }

    public WorkFactory WorkFactory()
    {
        return _workFactory;
    }

    public RatingFactory RatingFactory()
    {
        return _ratingFactory;
    }
}
