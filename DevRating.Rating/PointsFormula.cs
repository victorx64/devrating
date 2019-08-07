namespace DevRating.Rating
{
    public interface PointsFormula
    {
        double UpdatedPoints(double outcome, double points, double contender);
    }
}