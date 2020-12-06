using Semver;

namespace DevRating.VersionControl
{
    public interface Tag
    {
        string? Sha();
        SemVersion? Version();
    }
}