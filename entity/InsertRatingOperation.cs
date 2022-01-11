namespace devrating.entity;

public interface InsertRatingOperation
{
    Rating Insert(
        double value,
        Id previous,
        Id work,
        Id author
    );
}
