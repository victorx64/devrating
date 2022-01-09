namespace devrating.factory;

public sealed class DefaultFormula : Formula
{
    private readonly double _k;
    private readonly double _n;
    private readonly double _default;

    public DefaultFormula() : this(40d, 400d, 1500d)
    {
    }

    public DefaultFormula(double k, double n, double @default)
    {
        _k = k;
        _n = n;
        _default = @default;
    }

    public double DefaultRating()
    {
        return _default;
    }

    public double WinnerRatingChange(double winnerOldRating, double loserOldRating)
    {
        return _k * (1d - WinProbabilityOfA(winnerOldRating, loserOldRating));
    }

    private double WinProbabilityOfA(double a, double b)
    {
        var qa = Math.Pow(10d, a / _n);

        var qb = Math.Pow(10d, b / _n);

        var ea = qa / (qa + qb);

        return ea;
    }
}