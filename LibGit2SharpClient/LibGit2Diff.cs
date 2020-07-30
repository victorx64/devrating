// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DevRating.Domain;
using DevRating.VersionControl;
using LibGit2Sharp;
using Newtonsoft.Json;
using Diff = DevRating.Domain.Diff;

namespace DevRating.LibGit2SharpClient
{
    public sealed class LibGit2Diff : Diff, JsonObject
    {
        private readonly Commit _start;
        private readonly Commit _end;
        private readonly Envelope _since;
        private readonly Additions _additions;
        private readonly Deletions _deletions;
        private readonly string _key;
        private readonly string _organization;
        private readonly Envelope _link;

        public LibGit2Diff(
            string start,
            string end,
            Envelope since,
            IRepository repository,
            string key,
            Envelope link,
            string organization)
            : this(
                repository.Lookup<Commit>(start),
                repository.Lookup<Commit>(end),
                since,
                repository,
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
                start,
                end,
                since,
                new CachedHunks(new LibGit2Hunks(start, end, since, repository)),
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
            Hunks hunks,
            string key,
            Envelope link,
            string organization
        )
            : this(
                start,
                end,
                since,
                new TotalAdditions(hunks),
                new TotalDeletions(hunks),
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
            Additions additions,
            Deletions deletions,
            string key,
            Envelope link,
            string organization
        )
        {
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
                _end.Author.Email,
                _deletions.Items(),
                factory.InsertedWork(
                    _organization,
                    _key,
                    _start.Sha,
                    _end.Sha,
                    _since,
                    _end.Author.Email,
                    _additions.Count(),
                    _link,
                    createdAt
                ).Id(),
                createdAt
            );
        }

        public bool FullyCloned()
        {
            return _start != null && _end != null;
        }

        private class Dto
        {
            public string Author { get; set; } = string.Empty;
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
            return JsonConvert.SerializeObject(
                new Dto
                {
                    Additions = _additions.Count(),
                    Deletions = _deletions.Items().Select(deletion =>
                        new Dto.DeletionDto
                        {
                            Counted = deletion.Counted(),
                            Email = deletion.Email(),
                            Ignored = deletion.Ignored()
                        }
                    ),
                    End = _end.Sha,
                    Author = _end.Author.Email,
                    Key = _key,
                    Link = _link.Filled() ? _link.Value().ToString(CultureInfo.InvariantCulture) : null,
                    Organization = _organization,
                    Since = _since.Filled() ? _since.Value().ToString(CultureInfo.InvariantCulture) : null,
                    Start = _start.Sha
                }
            );
        }
    }
}