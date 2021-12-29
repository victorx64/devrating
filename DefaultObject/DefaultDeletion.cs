// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DevRating.Domain;

namespace DevRating.DefaultObject
{
    public sealed class DefaultDeletion : Deletion
    {
        private readonly string _email;
        private readonly uint _deletedLines;
        private readonly uint _availableLines;
        private readonly bool _accountable;

        public DefaultDeletion(string email, uint deletedLines, uint availableLines, bool accountable)
        {
            _email = email;
            _deletedLines = deletedLines;
            _availableLines = availableLines;
            _accountable = accountable;
        }

        public string Email()
        {
            return _email;
        }

        public uint DeletedLines()
        {
            return _deletedLines;
        }

        public bool Accountable()
        {
            return _accountable;
        }

        public uint AvailableLines()
        {
            return _availableLines;
        }
    }
}