using System;

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
    }
}