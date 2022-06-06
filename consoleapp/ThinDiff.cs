using devrating.entity;
using devrating.factory;

namespace devrating.consoleapp;

public sealed class ThinDiff : Diff
{
    private readonly string _commit;
    private readonly string _organization;
    private readonly string _repository;

    public ThinDiff(string organization, string repository, string commit)
    {
        _organization = organization;
        _repository = repository;
        _commit = commit;
    }

    public DateTimeOffset CreatedAt()
    {
        throw new NotImplementedException();
    }

    public Work NewWork(Factories factories)
    {
        throw new NotImplementedException();
    }

    public bool PresentIn(Works works)
    {
        return works.ContainsOperation().Contains(_organization, _repository, _commit);
    }

    public Work RelatedWork(Works works)
    {
        return works.GetOperation().Work(_organization, _repository, _commit);
    }

    public string ToJson()
    {
        throw new NotImplementedException();
    }
}
