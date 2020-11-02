// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using DevRating.Domain;
using Semver;

namespace DevRating.VersionControl
{
    public sealed class GitProcessBlames : AFileBlames
    {
        private IEnumerable<Blame>? _blames = null;
        private readonly Process _git;
        private readonly SemVersion _version;

        public GitProcessBlames(string path, string filename, string start, Envelope stop, SemVersion version)
            : this (path, filename, start, stop.Filled() ? stop.Value() + ".." : string.Empty, version)
        {
        }

        public GitProcessBlames(string path, string filename, string start, string stop, SemVersion version)
            : this (new VersionControlProcess("git", $"blame -t -e {stop}{start} -- \"{filename}\"", path), version)
        {
        }

        public GitProcessBlames(Process git, SemVersion version)
        {
            _git = git;
            _version = version;
        }

        public Blame AtLine(uint line)
        {
            if (_version.Major != 2)
            {
                throw new NotSupportedException("Required git version is 2.x.x");
            }

            _blames ??= BlameHunks(_git.Output());

            bool predicate(Blame b) 
            {
                return b.ContainsLine(line);
            }

            return _blames.Single(predicate);
        }

        private IEnumerable<Blame> BlameHunks(IList<string> lines)
        {
            var current = lines[0];
            var accum = 1u;

            for (var i = 1; i < lines.Count; i++)
            {
                var line = lines[i];

                if (i == lines.Count - 1 || !EqualShas(line, current))
                {
                    yield return OutOfRange(current)
                        ? (Blame)new IgnoredBlame(Email(current), (uint) i - accum, accum)
                        : (Blame)new CountedBlame(Email(current), (uint) i - accum, accum);

                    current = line;
                    accum = 1u;
                }
                else
                {
                    accum++;
                }
            }
        }

        private bool OutOfRange(string line)
        {
            return line.StartsWith("^");
        }

        private bool EqualShas(string a, string b)
        {
            return a.Substring(0, 8).Equals(b.Substring(0, 8), StringComparison.OrdinalIgnoreCase);
        }

        private string Email(string line)
        {
            return line.Substring(line.IndexOf('<') + 1, line.IndexOf('>') - line.IndexOf('<') - 1);
        }
    }
}