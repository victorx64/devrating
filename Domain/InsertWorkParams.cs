namespace DevRating.Domain
{
    public interface InsertWorkParams
    {
        Work InsertUsing(InsertWorkOperation operation, Entity author);
        Work InsertUsing(InsertWorkOperation operation, Entity author, Entity rating);
    }
}