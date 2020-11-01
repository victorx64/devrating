// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DevRating.Domain;

namespace DevRating.VersionControl
{
    public interface Blame
    {
        bool ContainsLine(uint line);
        Deletion SubDeletion(uint from, uint to);
    }
}