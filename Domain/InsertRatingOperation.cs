// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DevRating.Domain
{
    public interface InsertRatingOperation
    {
        Rating Insert(double value, Envelope counted, Envelope ignored, Id previous, Id work, Id author);
    }
}