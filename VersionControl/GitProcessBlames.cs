// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DevRating.VersionControl
{
    public sealed class GitProcessBlames : AFileBlames
    {
        private readonly string _path;
        private readonly string _since;
        private IEnumerable<Blame>? _blames = null;

        public GitProcessBlames(string path, string since)
        {
            _path = path;
            _since = since;
        }

        public Blame AtLine(uint line)
        {
            return (_blames ??= BlameHunks(GitBlameOutput()))
                .Single(b => b.ContainsLine(line));
        }

        private string[] GitBlameOutput()
        {
            var process = Process.Start(
                new ProcessStartInfo("git", $"blame -t -e {_since} -- \"{_path}\"")
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true
                }
            ) ?? throw new Exception("Process.Start() returned null");

            var output = process.StandardOutput
                .ReadToEnd()
                .Split('\n');

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception(process.StandardError.ReadToEnd());
            }

            return output;
        }

        private IEnumerable<Blame> BlameHunks(string[] lines)
        {
            if (!lines.Any())
            {
                yield break;
            }

            var current = lines[0];
            var accum = 1u;

            for (uint i = 1; i < lines.Length; i++)
            {
                var line = lines[i];

                if (!EqualShas(line, current))
                {
                    yield return EqualShas(line, _since)
                        ? (Blame)new IgnoredBlame(Email(line), i - accum, accum)
                        : (Blame)new CountedBlame(Email(line), i - accum, accum);

                    current = line;
                    accum = 1u;
                }
                else
                {
                    accum++;
                }
            }
        }

        private bool EqualShas(string a, string b)
        {
            return a.Substring(0, 8).Equals(b.Substring(0, 8), StringComparison.OrdinalIgnoreCase);
        }

        private string Email(string line)
        {
            return line.Substring(11, line.IndexOf('>') - 11);
        }
    }
}