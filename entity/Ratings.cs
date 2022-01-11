namespace devrating.entity;

public interface Ratings
{
    InsertRatingOperation InsertOperation();
    GetRatingOperation GetOperation();
    ContainsRatingOperation ContainsOperation();
}
