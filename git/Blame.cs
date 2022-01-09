using devrating.factory;

namespace devrating.git;

public interface Blame
{
    bool ContainsLine(uint line);
    ContemporaryLines SubDeletion(uint from, uint to);
}
