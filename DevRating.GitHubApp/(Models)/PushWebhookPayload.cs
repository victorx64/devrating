using System.Collections.Generic;
using System.Diagnostics;
using Octokit;

namespace DevRating.GitHubApp
{
    /// <summary>
    /// Delete this class when https://github.com/octokit/octokit.net/pull/1844 is merged
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class PushWebhookPayload : ActivityPayload
    {
        public string Head { get; protected set; } = string.Empty;
        public string Before { get; protected set; } = string.Empty;
        public string After { get; protected set; } = string.Empty;
        public string Ref { get; protected set; } = string.Empty;
        public string BaseRef { get; protected set; } = string.Empty;
        public bool Created { get; protected set; } = default;
        public bool Deleted { get; protected set; } = default;
        public bool Forced { get; protected set; } = default;
        public string Compare { get; protected set; } = string.Empty;
        public int Size { get; protected set; } = default;
        public IReadOnlyList<PushWebhookCommit> Commits { get; protected set; } = new PushWebhookCommit[] { };
        public PushWebhookCommit HeadCommit { get; protected set; } = new PushWebhookCommit();
    }
}