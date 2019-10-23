using System.Globalization;

namespace DevRating.GitHubApp.Models
{
    /// <summary>
    /// Delete this class when https://github.com/octokit/octokit.net/pull/1844 is merged
    /// </summary>
    public class PushWebhookCommitter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PushWebhookCommitter"/> class.
        /// </summary>
        public PushWebhookCommitter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PushWebhookCommitter"/> class.
        /// </summary>
        /// <param name="nodeId">The GraphQL Node Id</param>
        /// <param name="name">The full name of the author or committer.</param>
        /// <param name="email">The email.</param>
        /// <param name="username">The committer username.</param>
        public PushWebhookCommitter(string name, string email, string username)
        {
            Name = name;
            Email = email;
            Username = username;
        }

        /// <summary>Gets the name of the author or committer.</summary>
        /// <value>The name.</value>
        public string Name { get; protected set; } = string.Empty;

        /// <summary>Gets the email of the author or committer.</summary>
        /// <value>The email.</value>
        public string Email { get; protected set; } = string.Empty;

        /// <summary>
        /// Gets the username of the author or committer.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; protected set; } = string.Empty;

        internal string DebuggerDisplay
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "Name: {0} Email: {1} Username: {2}", Name, Email,
                    Username);
            }
        }
    }
}