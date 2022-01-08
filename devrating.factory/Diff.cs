using devrating.entity;

namespace devrating.factory;

public interface Diff
{
    Work RelatedWork(Works works);
    bool PresentIn(Works works);
    Work NewWork(Factories factories);
    string ToJson();
}
