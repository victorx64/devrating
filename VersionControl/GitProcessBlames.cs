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
        private readonly IEnumerable<Blame> _blames;

        public GitProcessBlames(string path, string filename, string start, Envelope stop, SemVersion version)
            : this(path, filename, start, stop.Filled() ? stop.Value() + ".." : string.Empty, version)
        {
        }

        public GitProcessBlames(string path, string filename, string start, string stop, SemVersion version)
            : this(new VersionControlProcess("git", $"blame -t -e -l {stop}{start} -- \"{filename}\"", path), version)
        {
        }

        public GitProcessBlames(Process git, SemVersion version)
            : this(git.Output(), version)
        {
        }

        public GitProcessBlames(IList<string> output, SemVersion version)
        {
            if (version.Major != 2)
            {
                throw new NotSupportedException("Required git version is 2.x.x");
            }

            _blames = BlameHunks(output).ToArray();
        }

        public Blame AtLine(uint line)
        {
            bool predicate(Blame b)
            {
                return b.ContainsLine(line);
            }

            return _blames.First(predicate);
        }

        private IEnumerable<Blame> BlameHunks(IList<string> lines)
        {
            var current = lines[0];
            var accum = 1u;

            for (var i = 1; i < lines.Count - 1; i++)
            {
                var line = lines[i];

                if (!EqualShas(line, current))
                {
                    yield return OutOfRange(current)
                        ? (Blame)new IgnoredBlame(Email(current), (uint)i - accum, accum)
                        : (Blame)new CountedBlame(Email(current), (uint)i - accum, accum);

                    current = line;
                    accum = 1u;
                }
                else
                {
                    accum++;
                }
            }

            if (lines.Count > 1)
            {
                var i = lines.Count - 1;

                yield return OutOfRange(current)
                    ? (Blame)new IgnoredBlame(Email(current), (uint)i - accum, accum)
                    : (Blame)new CountedBlame(Email(current), (uint)i - accum, accum);
            }
        }

        /// https://git-scm.com/docs/git-blame#_specifying_ranges
        private bool OutOfRange(string line)
        {
            return line[0] == '^';
        }

        private bool EqualShas(string a, string b)
        {
            return a.Substring(0, 40).Equals(b.Substring(0, 40), StringComparison.Ordinal);
        }

        private string Email(string line)
        {
            var start = line.IndexOf('<');
            return line.Substring(start + 1, line.IndexOf('>') - start - 1);
        }
    }
}