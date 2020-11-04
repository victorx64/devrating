// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using DevRating.DefaultObject;
using DevRating.Domain;
using Semver;

namespace DevRating.VersionControl
{
    public sealed class GitProcessVersion : Tag
    {
        private readonly SemVersion _version;

        public GitProcessVersion()
            : this (new VersionControlProcess("git", "version", "."))
        {
        }

        public GitProcessVersion(Process git)
            : this (git.Output()[0])
        {
        }

        public GitProcessVersion(string output)
            : this(output.Substring(12).Split('.'))
        {
        }

        public GitProcessVersion(string[] fragments)
            : this(
                new SemVersion(
                    int.Parse(fragments[0], CultureInfo.InvariantCulture),
                    int.Parse(fragments[1], CultureInfo.InvariantCulture),
                    int.Parse(fragments[2], CultureInfo.InvariantCulture)
                )
            )
        {
        }

        public GitProcessVersion(SemVersion version)
        {
            _version = version;
        }

        public bool HasVersion()
        {
            return true;
        }

        public Envelope Sha()
        {
            return new DefaultEnvelope();
        }

        public SemVersion Version()
        {
            return _version;
        }
    }
}