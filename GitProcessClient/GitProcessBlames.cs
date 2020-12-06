// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using DevRating.Domain;
using DevRating.VersionControl;

namespace DevRating.GitProcessClient
{
    public sealed class GitProcessBlames : AFileBlames
    {
        private IEnumerable<Blame>? _blames;
        private readonly object _lock = new object();
        private readonly Process _git;
        private readonly string? _stop;

        public GitProcessBlames(string path, string filename, string start, string? stop)
            : this(
                new VersionControlProcess("git", $"blame -t -e -l {(stop is object ? stop + ".." : "")}{start} -- \"{filename}\"", path),
                stop
            )
        {
        }

        public GitProcessBlames(Process git, string? stop)
        {
            _git = git;
            _stop = stop;
        }

        public Blame AtLine(uint line)
        {
            bool predicate(Blame b)
            {
                return b.ContainsLine(line);
            }

            return InitedBlames().First(predicate);
        }

        private IEnumerable<Blame> InitedBlames()
        {
            if (_blames == null)
            {
                lock (_lock)
                {
                    if (_blames == null)
                    {
                        var output = _git.Output();

                        _blames = _stop is object
                            ? BlameHunks(output, _stop).ToArray()
                            : BlameHunks(output).ToArray();
                    }
                }
            }

            return _blames;
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