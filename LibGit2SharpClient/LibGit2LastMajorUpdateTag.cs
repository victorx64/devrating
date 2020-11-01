// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using DevRating.Domain;
using DevRating.VersionControl;
using LibGit2Sharp;
using Semver;
using Tag = DevRating.VersionControl.Tag;
using System;

namespace DevRating.LibGit2SharpClient
{
    public sealed class LibGit2LastMajorUpdateTag : Tag
    {
        private readonly Tag _release;

        public LibGit2LastMajorUpdateTag(IRepository repository, string before)
            : this(
                new LastMajorUpdateTag(
                    repository
                    .Refs
                    .ReachableFrom(
                        new [] {
                            repository.Lookup<Commit>(before)
                                ?? throw new ArgumentNullException(nameof(before), $"Commit `{before}` not found.")
                        }
                    )
                    .Where(
                        delegate(Reference r)
                        {
                            return r.IsTag;
                        }
                    )
                    .Select(
                        delegate(Reference r)
                        {
                            return new VersionControlTag(r.TargetIdentifier, r.CanonicalName);
                        }
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