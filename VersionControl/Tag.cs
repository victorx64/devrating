using DevRating.Domain;
using Semver;

namespace DevRating.VersionControl
{
    public interface Tag
    {
        Envelope Sha();
        bool HasVersion();
        SemVersion Version();
    }
}