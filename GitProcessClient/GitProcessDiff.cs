// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DevRating.Domain;
using DevRating.VersionControl;

namespace DevRating.GitProcessClient
{
    public sealed class GitProcessDiff : Diff
    {
        private readonly string _start;
        private readonly string _end;
        private readonly string? _since;
        private readonly Additions _additions;
        private readonly Deletions _deletions;
        private readonly string _key;
        private readonly string _organization;
        private readonly string? _link;
        private readonly string _email;

        public GitProcessDiff(
            string email,
            string start,
            string end,
            string? since,
            string repository,
            string key,
            string? link,
            string organization
        )
            : this(
                email,
                start,
                end,
                since,
                repository,
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
            string? since,
            string repository,
            string key,
            string? link,
            string organization
        )
            : this(
                new VersionControl.VersionControlProcess("git", $"show -s --format=%aE {end}", repository).Output()[0],
                start,
                end,
                since,
                repository,
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
            string? since,
            string repository,
            Patches patches,
            string key,
            string? link,
            string organization
        )
            : this(
                email,
                new VersionControl.VersionControlProcess("git", $"rev-parse {start}", repository).Output()[0],
                new VersionControl.VersionControlProcess("git", $"rev-parse {end}", repository).Output()[0],
                since,
                new TotalAdditions(patches),
                new TotalDeletions(patches),
                key,
                link,
                organization
            )
        {
        }

        private GitProcessDiff(
            string email,
            string start,
            string end,
            string? since,
            Additions additions,
            Deletions deletions,
            string key,
            string? link,
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

        private class Dto
        {
            public string Email { get; set; } = string.Empty;
            public string Start { get; set; } = string.Empty;
            public string End { get; set; } = string.Empty;
            public string Organization { get; set; } = string.Empty;
            public string? Since { get; set; }
            public string Key { get; set; } = string.Empty;
            public string? Link { get; set; }
            public uint Additions { get; set; }
            public IEnumerable<DeletionDto> Deletions { get; set; } = new DeletionDto[0];

            internal class DeletionDto
            {
                public string Email { get; set; } = string.Empty;
                public uint Counted { get; set; }
                public uint Ignored { get; set; }
            }
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize<Dto>(
                new Dto
                {
                    Additions = _additions.Count(),
                    Deletions = _deletions.Items().Select(
                        deletion => new Dto.DeletionDto
                        {
                            Counted = deletion.Counted(),
                            Email = deletion.Email(),
                            Ignored = deletion.Ignored()
                        }
                    ),
                    End = _end,
                    Email = _email,
                    Key = _key,
                    Link = _link,
                    Organization = _organization,
                    Since = _since,
                    Start = _start
                }
            );
        }
    }
}