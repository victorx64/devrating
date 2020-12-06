// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeWork : Work
    {
        private readonly Id _id;
        private readonly uint _additions;
        private readonly Author _author;
        private readonly Rating _rating;
        private readonly string _repository;
        private readonly string _start;
        private readonly string _end;
        private readonly string? _since;
        private readonly string? _link;
        private readonly DateTimeOffset _createdAt;

        public FakeWork(uint additions, Author author)
            : this(
                additions,
                author,
                "repository",
                "startCommit",
                "endCommit",
                "sinceCommit",
                DateTimeOffset.UtcNow,
                "link"
            )
        {
        }

        public FakeWork(
            uint additions,
            Author author,
            string repository,
            string start,
            string end,
            string? since,
            DateTimeOffset createdAt,
            string? link
        )
            : this(
                additions,
                author,
                new NullRating(),
                repository,
                start,
                end,
                since,
                createdAt,
                link
            )
        {
        }

        public FakeWork(
            uint additions,
            Author author,
            Rating rating,
            string repository,
            string start,
            string end,
            string? since,
            DateTimeOffset createdAt,
            string? link
        )
            : this(
                new DefaultId(Guid.NewGuid()),
                additions,
                author,
                rating,
                repository,
                start,
                end,
                since,
                createdAt,
                link
            )
        {
        }

        public FakeWork(
            Id id,
            uint additions,
            Author author,
            Rating rating,
            string repository,
            string start,
            string end,
            string? since,
            DateTimeOffset createdAt,
            string? link
        )
        {
            _id = id;
            _additions = additions;
            _author = author;
            _rating = rating;
            _repository = repository;
            _start = start;
            _end = end;
            _since = since;
            _createdAt = createdAt;
            _link = link;
        }

        public Id Id()
        {
            return _id;
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }

        public uint Additions()
        {
            return _additions;
        }

        public Author Author()
        {
            return _author;
        }

        public Rating UsedRating()
        {
            return _rating;
        }

        public string Repository()
        {
            return _repository;
        }

        public string Start()
        {
            return _start;
        }

        public string End()
        {
            return _end;
        }

        public string? Since()
        {
            return _since;
        }

        public DateTimeOffset CreatedAt()
        {
            return _createdAt;
        }

        public string? Link()
        {
            return _link;
        }
    }
}