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
            : this(
                new VersionControlProcess("git", $"blame -t -e -l {(stop.Filled() ? stop.Value() + ".." : "")}{start} -- \"{filename}\"", path).Output(), 
                version, 
                stop
            )
        {
        }

        public GitProcessBlames(IList<string> output, SemVersion version, Envelope stop)
        {
            if (version.Major != 2)
            {
                throw new NotSupportedException("Required git version is 2.x.x");
            }

            _blames = stop.Filled()
                ? BlameHunks(output, stop.Value().ToString()!).ToArray()
                : BlameHunks(output).ToArray();
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
                    yield return (Blame)new CountedBlame(Email(current), (uint)i - accum, accum);

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

                yield return (Blame)new CountedBlame(Email(current), (uint)i - accum, accum);
            }
        }

        private IEnumerable<Blame> BlameHunks(IList<string> lines, string stop)
        {
            var current = lines[0];
            var accum = 1u;
            var since = "^" + stop.Substring(0, 39);

            for (var i = 1; i < lines.Count - 1; i++)
            {
                var line = lines[i];

                if (!EqualShas(line, current))
                {
                    yield return current.StartsWith(since, StringComparison.Ordinal)
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

                yield return current.StartsWith(since, StringComparison.Ordinal)
                    ? (Blame)new IgnoredBlame(Email(current), (uint)i - accum, accum)
                    : (Blame)new CountedBlame(Email(current), (uint)i - accum, accum);
            }
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