using System.Linq;
using DevRating.Domain;
using DevRating.VersionControl;
using LibGit2Sharp;
using Semver;
using Tag = DevRating.VersionControl.Tag;

namespace DevRating.LibGit2SharpClient
{
    public sealed class LibGit2LastMajorUpdateTag : Tag
    {
        private readonly Tag _release;

        public LibGit2LastMajorUpdateTag(IRepository repository)
            : this(
                new LastMajorUpdateTag(
                    repository.Tags.Select(
                        delegate(LibGit2Sharp.Tag t) { return new VersionControlTag(t.PeeledTarget.Sha, t.FriendlyName); }
                    )
                )
            )
        {
        }

        private LibGit2LastMajorUpdateTag(Tag release)
        {
            _release = release;
        }

        public Envelope Sha()
        {
            return _release.Sha();
        }

        public bool HasVersion()
        {
            return _release.HasVersion();
        }

        public SemVersion Version()
        {
            return _release.Version();
        }
    }
}