// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevRating.VersionControl;

namespace DevRating.GitProcessClient
{
    public sealed class GitProcessPatches : Patches
    {
        private readonly string _start;
        private readonly string _end;
        private readonly string? _since;
        private readonly string _repository;

        public GitProcessPatches(string start, string end, string? since, string repository)
        {
            _start = start;
            _end = end;
            _repository = repository;
            _since = since;
        }

        public IEnumerable<FilePatch> Items()
        {
            return Task.WhenAll(ItemTasks())
                .GetAwaiter()
                .GetResult();
        }

        private enum State
        {
            Diff,
            OldPath
        }

        private IEnumerable<Task<FilePatch>> ItemTasks()
        {
            var patch = new List<string>();
            var old = "unknown";
            var state = State.Diff;

            foreach (var line in new VersionControlProcess("git", $"diff {_start}..{_end} -U0 -M01 -w", _repository).Output())
            {
                switch (state)
                {
                    case State.Diff:
                        if (line.StartsWith("--- ", StringComparison.Ordinal))
                        {
                            old = "." + line.Substring(5);
                            state = State.OldPath;
                        }

                        break;
                    case State.OldPath:
                        if (line.StartsWith("diff --git ", StringComparison.Ordinal))
                        {
                            yield return FilePatchTask(patch.ToArray(), old.ToString());

                            old = "empty";
                            patch.Clear();
                            state = State.Diff;
                        }

                        break;
                }

                patch.Add(line);
            }

            yield return FilePatchTask(patch.ToArray(), old.ToString());
        }

        private Task<FilePatch> FilePatchTask(IEnumerable<string> patch, string old)
        {
            return Task.Run(
                () => (FilePatch) new VersionControlFilePatch(
                    patch,
                    new GitProcessBlames(
                        _repository,
                        old,
                        _start,
                        _since
                    )
                )
            );
        }
    }
}