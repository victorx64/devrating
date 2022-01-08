namespace devrating.entity;

public interface Work : Entity
{
    Author Author();
    Rating UsedRating();
    string Start();
    string End();
    string? Link();
    string? Since();
    DateTimeOffset CreatedAt();
}
