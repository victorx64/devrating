using System.Collections.Generic;

namespace DevRating.GitHubApp
{
    public class PushEventPayload
    {
        public IList<Commit> Commits { get; set; } = new List<Commit>();

        public Installation Installation { get; set; } = new Installation();
    }
}