// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace DevRating.LibGit2SharpClient
{
    public sealed class GitProcessMailmap : Mailmap
    {
        private readonly IDictionary<string, string> _map;
        private readonly object _lock = new object();
        private readonly string _path;

        public GitProcessMailmap(string path) : this(path, new Dictionary<string, string>())
        {
        }

        public GitProcessMailmap(string path, IDictionary<string, string> map)
        {
            _path = path;
            _map = map;
        }

        public string MappedEmail(string email)
        {
            lock (_lock)
            {
                if (!_map.ContainsKey(email))
                {
                    _map.Add(
                        email,
                        new VersionControl.VersionControlProcess("git", $"check-mailmap \"<{email}>\"", _path)
                            .Output()[0]
                            .TrimStart('<')
                            .TrimEnd('>')
                    );
                }

                return _map[email];
            }
        }
    }
}