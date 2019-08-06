namespace DevRating.Rating
{
    public interface PointsFormula
    {
        double UpdatedPoints(double outcome, Player player, Player contender);
    }
}