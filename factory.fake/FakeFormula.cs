namespace devrating.factory.fake;

public sealed class FakeFormula : Formula
{
    private readonly double _default;
    private readonly double _increase;
    private readonly double _n;

    public FakeFormula() : this(10d, 1d, 400)
    {
    }

    public FakeFormula(double @default, double increase)
        : this(@default, increase, 400)
    {
    }

    public FakeFormula(double @default, double increase, double n)
    {
        _default = @default;
        _increase = increase;
        _n = n;
    }

    public double DefaultRating()
    {
        return _default;
    }

    public double SuggestedAdditionsCount(double rating)
    {
        return 1d - WinProbabilityOfA(rating, _default);
    }

    public double WinnerRatingChange(double winnerOldRating, double loserOldRating)
    {
        return _increase;
    }

    private double WinProbabilityOfA(double a, double b)
    {
        var qa = Math.Pow(10d, a / _n);

        var qb = Math.Pow(10d, b / _n);

        var ea = qa / (qa + qb);

        return ea;
    }
}
