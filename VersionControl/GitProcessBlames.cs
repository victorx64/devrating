// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DevRating.Domain;

namespace DevRating.VersionControl
{
    public sealed class GitProcessBlames : AFileBlames
    {
        private readonly string _path;
        private readonly string _filename;
        private readonly string _stop;
        private readonly string _start;
        private IEnumerable<Blame>? _blames = null;

        public GitProcessBlames(string path, string filename, string start, Envelope stop)
            : this (path, filename, start, stop.Filled() ? stop.Value() + ".." : string.Empty)
        {
        }

        public GitProcessBlames(string path, string filename, string start, string stop)
        {
            _path = path;
            _filename = filename;
            _start = start;
            _stop = stop;
        }

        public Blame AtLine(uint line)
        {
            _blames ??= BlameHunks(GitBlameOutput());

            return _blames.Single(b => b.ContainsLine(line));
        }

        private string[] GitBlameOutput()
        {
            var process = Process.Start(
                new ProcessStartInfo("git", $"blame -t -e {_stop}{_start} -- \"{_filename}\"")
                {
                    WorkingDirectory = _path,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true
                }
            ) 
            ?? throw new InvalidOperationException("Process.Start() returned null");

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
                
                if (i == lines.Length - 1 || !EqualShas(line, current))
                {
                    yield return OutOfRange(current)
                        ? (Blame)new IgnoredBlame(Email(current), i - accum, accum)
                        : (Blame)new CountedBlame(Email(current), i - accum, accum);

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
            return line.Substring(11, line.IndexOf('>') - 11);
        }
    }
}