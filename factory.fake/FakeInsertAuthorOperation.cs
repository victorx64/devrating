using devrating.entity;

namespace devrating.factory.fake;

public sealed class FakeInsertAuthorOperation : InsertAuthorOperation
{
    private readonly IList<Author> _authors;

    public FakeInsertAuthorOperation(IList<Author> authors)
    {
        _authors = authors;
    }

    public Author Insert(string organization, string repository, string email, DateTimeOffset createdAt)
    {
        var author = new FakeAuthor(organization, repository, email, createdAt);

        _authors.Add(author);

        return author;
    }
}
