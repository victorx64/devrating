using Semver;

namespace DevRating.VersionControl
{
    public interface Tag
    {
        string? Sha();
        bool HasVersion();
        SemVersion Version();
    }
}