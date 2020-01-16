using System;
using System.Runtime.Serialization;

namespace DevRating.LibGit2SharpClient
{
    [Serializable]
    public sealed class EncounteredNonContextualLineException : Exception
    {
        public EncounteredNonContextualLineException()
            : this("Patch must be without contextual lines. Add '-U0' option to git calls.")
        {
        }

        public EncounteredNonContextualLineException(string message) : base(message)
        {
        }

        public EncounteredNonContextualLineException(string message, Exception inner) : base(message, inner)
        {
        }

        private EncounteredNonContextualLineException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}