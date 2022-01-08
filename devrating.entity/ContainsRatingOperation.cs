namespace devrating.entity;

public interface ContainsRatingOperation
{
    bool ContainsRatingOf(Id author);
    bool Contains(Id id);
}
