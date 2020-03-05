// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DevRating.VersionControl
{
    public interface Blame
    {
        string AuthorEmail();
        uint StartLineNumber();
        uint LineCount();
        bool ContainsLine(uint line);
    }
}