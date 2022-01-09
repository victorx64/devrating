namespace devrating.factory.fake;

public sealed class FakeFormula : Formula
{
    private readonly double _default;
    private readonly double _increase;

    public FakeFormula() : this(10d, 1d)
    {
    }

    public FakeFormula(double @default, double increase)
    {
        _default = @default;
        _increase = increase;
    }

    public double DefaultRating()
    {
        return _default;
    }

    public double WinnerRatingChange(double winnerOldRating, double loserOldRating)
    {
        return _increase;
    }
}
