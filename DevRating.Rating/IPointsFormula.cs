namespace DevRating.Rating
{
    public interface IPointsFormula
    {
        double UpdatedPoints(double outcome, IPlayer player, IPlayer opponent);
    }
}