using devrating.entity;

namespace devrating.factory.fake;

public sealed class FakeAuthor : Author
{
    private readonly Id _id;
    private readonly string _email;
    private readonly string _organization;
    private readonly string _repository;
    private readonly DateTimeOffset _createdAt;

    public FakeAuthor(string organization, string repository, string email)
        : this(organization, repository, email, DateTimeOffset.UtcNow)
    {
    }

    public FakeAuthor(string organization, string repository, string email, DateTimeOffset createdAt)
        : this(new DefaultId(Guid.NewGuid()), email, organization, repository, createdAt)
    {
    }

    public FakeAuthor(Id id, string email, string organization, string repository, DateTimeOffset createdAt)
    {
        _id = id;
        _email = email;
        _organization = organization;
        _createdAt = createdAt;
        _repository = repository;
    }

    public Id Id()
    {
        return _id;
    }

    public string ToJson()
    {
        throw new NotImplementedException();
    }

    public string Email()
    {
        return _email;
    }

    public string Organization()
    {
        return _organization;
    }

    public DateTimeOffset CreatedAt()
    {
        return _createdAt;
    }

    public string Repository()
    {
        return _repository;
    }
}
