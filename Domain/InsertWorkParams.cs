namespace DevRating.Domain
{
    public interface InsertWorkParams
    {
        Work InsertionResult(InsertWorkOperation operation, Entity author);
        Work InsertionResult(InsertWorkOperation operation, Entity author, Entity rating);
    }
}