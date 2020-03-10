using System.Collections.Generic;
using System.Linq;
using DevRating.DefaultObject;
using DevRating.Domain;
using Semver;

namespace DevRating.VersionControl
{
    public sealed class LastMajorUpdateTag : Tag
    {
        private readonly Tag? _release;

        public LastMajorUpdateTag(IEnumerable<Tag> releases)
            : this(
                releases.Where(delegate(Tag r) { return r.HasVersion(); })
                    .OrderByDescending(delegate(Tag r) { return r.Version(); })
                    .ToList()
            )
        {
        }

        private LastMajorUpdateTag(IList<Tag> releases)
            : this(
                releases.Any() && releases.First().Version().Major != releases.Last().Version().Major
                    ? releases.Last(delegate(Tag r) { return r.Version().Major == releases.First().Version().Major; })
                    : null
            )
        {
        }

        private LastMajorUpdateTag(Tag? release)
        {
            _release = release;
        }

        public Envelope Sha()
        {
            return _release != null ? _release.Sha() : new DefaultEnvelope();
        }

        public bool HasVersion()
        {
            return _release != null;
        }

        public SemVersion Version()
        {
            return _release!.Version();
        }
    }
}