using devrating.entity;
using devrating.factory;
using devrating.factory.fake;

namespace devrating.consoleapp.fake;

public sealed class FakeDiff : Diff
{
    private readonly string _key;
    private readonly string _commit;
    private readonly string? _since;
    private readonly string? _link;
    private readonly string _email;
    private readonly string _organization;
    private readonly IEnumerable<ContemporaryLines> _deletions;
    private readonly DateTimeOffset _createdAt;

    public FakeDiff(string? link)
        : this(
            "key",
            "commit",
            "since this commit",
            "author",
            "org",
            new[]
            {
                new FakeContemporaryLines("victim1", 7u),
                new FakeContemporaryLines("victim2", 14u),
            },
            DateTimeOffset.UtcNow,
            link
        )
    {
    }

    public FakeDiff()
        : this(
            "key",
            "commit",
            "since this commit",
            "author",
            "org",
            new[]
            {
                new FakeContemporaryLines("victim1", 7u),
                new FakeContemporaryLines("victim2", 14u),
            },
            DateTimeOffset.UtcNow
        )
    {
    }

    public FakeDiff(
        string key,
        string commit,
        string? since,
        string email,
        string organization,
        IEnumerable<ContemporaryLines> deletions,
        DateTimeOffset createdAt)
        : this(
            key,
            commit,
            since,
            email,
            organization,
            deletions,
            createdAt,
            null
        )
    {
    }

    public FakeDiff(
        string key,
        string commit,
        string? since,
        string email,
        string organization,
        IEnumerable<ContemporaryLines> deletions,
        DateTimeOffset createdAt,
        string? link
    )
    {
        _key = key;
        _commit = commit;
        _since = since;
        _email = email;
        _organization = organization;
        _deletions = deletions;
        _createdAt = createdAt;
        _link = link;
    }

    public Work RelatedWork(Works works)
    {
        return works.GetOperation().Work(_organization, _key, _commit);
    }

    public bool PresentIn(Works works)
    {
        return works.ContainsOperation().Contains(_organization, _key, _commit);
    }

    public Work NewWork(Factories factories)
    {
        var work = factories.WorkFactory().NewWork(
            _organization,
            _key,
            _commit,
            _since,
            _email,
            _link,
            _createdAt,
            Array.Empty<string>()
        );

        factories.RatingFactory().NewRatings(
            _organization,
            _key,
            _email,
            _deletions,
            work.Id(),
            _createdAt
        );

        return work;
    }

    public string ToJson()
    {
        throw new NotImplementedException();
    }

    public uint Additions()
    {
        throw new NotImplementedException();
    }
}
