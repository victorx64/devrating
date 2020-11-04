// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using DevRating.Domain;
using DevRating.VersionControl;
using LibGit2Sharp;
using Diff = DevRating.Domain.Diff;

namespace DevRating.LibGit2SharpClient
{
    public sealed class LibGit2Diff : Diff
    {
        private readonly Commit _start;
        private readonly Commit _end;
        private readonly Envelope _since;
        private readonly Additions _additions;
        private readonly Deletions _deletions;
        private readonly string _key;
        private readonly string _organization;
        private readonly Envelope _link;
        private readonly string _email;

        public LibGit2Diff(
            string start,
            string end,
            Envelope since,
            IRepository repository,
            string key,
            Envelope link,
            string organization)
            : this(
                repository.Lookup<Commit>(start)
                    ?? throw new ArgumentNullException(nameof(start), $"Start commit `{start}` not found."),
                repository.Lookup<Commit>(end)
                    ?? throw new ArgumentNullException(nameof(end), $"End commit `{end}` not found."),
                since,
                repository,
                key,
                link,
                organization
            )
        {
        }

        public LibGit2Diff(
            string email,
            string start,
            string end,
            Envelope since,
            IRepository repository,
            string key,
            Envelope link,
            string organization)
            : this(
                email,
                repository.Lookup<Commit>(start)
                    ?? throw new ArgumentNullException(nameof(start), $"Start commit `{start}` not found."),
                repository.Lookup<Commit>(end)
                    ?? throw new ArgumentNullException(nameof(end), $"End commit `{end}` not found."),
                since,
                repository,
                key,
                link,
                organization
            )
        {
        }

        public LibGit2Diff(
            string email,
            Commit start,
            Commit end,
            Envelope since,
            IRepository repository,
            string key,
            Envelope link,
            string organization
        )
            : this(
                email,
                start,
                end,
                since,
                new CachedPatches(new GitProcessPatches(start, end, since, repository)),
                key,
                link,
                organization
            )
        {
        }

        public LibGit2Diff(
            Commit start,
            Commit end,
            Envelope since,
            IRepository repository,
            string key,
            Envelope link,
            string organization
        )
            : this(
                end.Author.Email,
                start,
                end,
                since,
                new CachedPatches(new GitProcessPatches(start, end, since, repository)),
                key,
                link,
                organization
            )
        {
        }

        public LibGit2Diff(
            string email,
            Commit start,
            Commit end,
            Envelope since,
            Patches patches,
            string key,
            Envelope link,
            string organization
        )
            : this(
                email,
                start,
                end,
                since,
                new TotalAdditions(patches),
                new TotalDeletions(patches),
                key,
                link,
                organization
            )
        {
        }

        public LibGit2Diff(
            string email,
            Commit start,
            Commit end,
            Envelope since,
            Additions additions,
            Deletions deletions,
            string key,
            Envelope link,
            string organization
        )
        {
            _email = email;
            _start = start;
            _end = end;
            _since = since;
            _additions = additions;
            _deletions = deletions;
            _key = key;
            _link = link;
            _organization = organization;
        }

        public Work From(Works works)
        {
            return works.GetOperation().Work(_key, _start.Sha, _end.Sha);
        }

        public bool PresentIn(Works works)
        {
            return works.ContainsOperation().Contains(_key, _start.Sha, _end.Sha);
        }

        public void AddTo(EntityFactory factory, DateTimeOffset createdAt)
        {
            factory.InsertRatings(
                _organization,
                _email,
                _deletions.Items(),
                factory.InsertedWork(
                    _organization,
                    _key,
                    _start.Sha,
                    _end.Sha,
                    _since,
                    _email,
                    _additions.Count(),
                    _link,
                    createdAt
                ).Id(),
                createdAt
            );
        }
    }
}