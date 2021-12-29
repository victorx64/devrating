// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using DevRating.VersionControl;

namespace DevRating.GitProcessClient
{
    public sealed class GitProcessDiffSizes : DiffSizes
    {
        private readonly Dictionary<string, uint> _additions = new Dictionary<string, uint>();
        private readonly object _lock = new object();
        private readonly string _repository;

        public GitProcessDiffSizes(string repository)
        {
            _repository = repository;
        }

        public uint Additions(string sha)
        {
            if (!_additions.ContainsKey(sha))
            {
                lock (_lock)
                {
                    if (!_additions.ContainsKey(sha))
                    {
                        // Keep git arguments in sync with GitProcessPatches and GitProcessBlames
                        var stat = new VersionControlProcess(
                            "git",
                            $"diff {sha} -U0 -M01 -w --shortstat",
                            _repository
                        ).Output()[0];

                        //  20 files changed, 77 insertions(+), 182 deletions(-)
                        var start = stat.IndexOf("changed, " + "changed, ".Length);
                        var end = stat.IndexOf(" insertions");

                        var insertions = uint.Parse(stat.Substring(start, end - start));

                        _additions[sha] = insertions;
                    }
                }
            }

            return _additions[sha];
        }
    }
}