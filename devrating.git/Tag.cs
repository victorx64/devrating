using Semver;

namespace devrating.git;

public interface Tag
{
    string? Sha();
    SemVersion? Version();
}
