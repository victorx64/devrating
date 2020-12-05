// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using DevRating.Domain;
using DevRating.VersionControl;

namespace DevRating.GitProcessClient
{
    public sealed class GitProcessDiff : Diff
    {
        private readonly string _start;
        private readonly string _end;
        private readonly Envelope _since;
        private readonly Additions _additions;
        private readonly Deletions _deletions;
        private readonly string _key;
        private readonly string _organization;
        private readonly Envelope _link;
        private readonly string _email;

        public GitProcessDiff(
            string email,
            string start,
            string end,
            Envelope since,
            string repository,
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

        public GitProcessDiff(
            string start,
            string end,
            Envelope since,
            string repository,
            string key,
            Envelope link,
            string organization
        )
            : this(
                new VersionControl.VersionControlProcess("git", $"show -s --format=%ae {end}", repository).Output()[0],
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

        public GitProcessDiff(
            string email,
            string start,
            string end,
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

        public GitProcessDiff(
            string email,
            string start,
            string end,
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
            return works.GetOperation().Work(_key, _start, _end);
        }

        public bool PresentIn(Works works)
        {
            return works.ContainsOperation().Contains(_key, _start, _end);
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
                    _start,
                    _end,
                    _since,
                    _email,
                    _additions.Count(),
                    _link,
                    createdAt
                )
                .Id(),
                createdAt
            );
        }
    }
}