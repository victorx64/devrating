using devrating.entity;
using devrating.factory;

namespace devrating.consoleapp;

public sealed class ThinDiff : Diff
{
    private readonly string _start;
    private readonly string _end;
    private readonly string _organization;
    private readonly string _repository;

    public ThinDiff(string organization, string repository, string start, string end)
    {
        _organization = organization;
        _repository = repository;
        _start = start;
        _end = end;
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
        return works.ContainsOperation().Contains(_organization, _repository, _start, _end);
    }

    public Work RelatedWork(Works works)
    {
        return works.GetOperation().Work(_organization, _repository, _start, _end);
    }

    public string ToJson()
    {
        throw new NotImplementedException();
    }
}
