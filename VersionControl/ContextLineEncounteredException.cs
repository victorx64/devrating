// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace DevRating.VersionControl
{
    [Serializable]
    public sealed class ContextLineEncounteredException : Exception
    {
        public ContextLineEncounteredException()
            : this("Patch must be without contextual lines. Add '-U0' option to git calls.")
        {
        }

        public ContextLineEncounteredException(string message) : base(message)
        {
        }

        public ContextLineEncounteredException(string message, Exception inner) : base(message, inner)
        {
        }

        private ContextLineEncounteredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}