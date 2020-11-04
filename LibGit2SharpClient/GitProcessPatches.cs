// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevRating.Domain;
using DevRating.VersionControl;
using LibGit2Sharp;
using Semver;
using CompareOptions = LibGit2Sharp.CompareOptions;

namespace DevRating.LibGit2SharpClient
{
    public sealed class GitProcessPatches : Patches
    {
        private readonly Commit _start;
        private readonly Commit _end;
        private readonly Envelope _since;
        private readonly IRepository _repository;
        private readonly SemVersion _version;

        public GitProcessPatches(Commit start, Commit end, Envelope since, IRepository repository)
        : this (start, end, since, repository, new GitProcessVersion().Version())
        {
        }

        public GitProcessPatches(Commit start, Commit end, Envelope since, IRepository repository, SemVersion version)
        {
            _start = start;
            _end = end;
            _repository = repository;
            _since = since;
            _version = version;
        }

        public IEnumerable<FilePatch> Items()
        {
            return Task.WhenAll(ItemTasks())
                .GetAwaiter()
                .GetResult();
        }

        private IEnumerable<Task<FilePatch>> ItemTasks()
        {
            var differences = _repository.Diff.Compare<Patch>(_start.Tree, _end.Tree,
                new CompareOptions { ContextLines = 0 });

            foreach (var difference in differences.Where(IsModification))
            {
                FilePatch Function()
                {
                    return new VersionControlFilePatch(
                        difference!.Patch,
                        new GitProcessBlames(
                            _repository.Info.WorkingDirectory ?? _repository.Info.Path,
                            difference.OldPath,
                            _start.Sha,
                            _since,
                            _version
                        )
                    );
                }

                yield return Task.Run(Function);
            }

            foreach (var difference in differences.Where(IsCreation))
            {
                yield return Task.FromResult(
                    (FilePatch)new VersionControlFilePatch(
                        new EmptyDeletions(),
                        new VersionControlAdditions(difference.Patch)
                    )
                );
            }
        }

        private bool IsModification(PatchEntryChanges changes)
        {
            return !changes.IsBinaryComparison &&
                changes.OldMode == Mode.NonExecutableFile &&
                (changes.Mode == Mode.NonExecutableFile && changes.Status == ChangeKind.Modified || 
                changes.Mode == Mode.Nonexistent && changes.Status == ChangeKind.Deleted);
        }

        private bool IsCreation(PatchEntryChanges changes)
        {
            return !changes.IsBinaryComparison &&
                changes.Mode == Mode.NonExecutableFile &&
                changes.OldMode == Mode.Nonexistent &&
                changes.Status == ChangeKind.Added;
        }
    }
}