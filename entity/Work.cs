namespace devrating.entity;

public interface Work : Entity
{
    Author Author();
    Rating UsedRating();
    string Commit();
    string? Link();
    string? Since();
    DateTimeOffset CreatedAt();
    string? Paths();
}
