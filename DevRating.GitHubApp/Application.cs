using System;
using System.IO;
using System.Threading.Tasks;
using DevRating.GitHubApp.Models;
using DevRating.LibGit2Sharp;
using DevRating.Rating;
using DevRating.SqlClient;
using LibGit2Sharp;
using Octokit;

namespace DevRating.GitHubApp
{
    public sealed class Application
    {
        private readonly JsonWebToken _token;
        private readonly string _name;
        private readonly string _connection;

        public Application(JsonWebToken token, string name, string connection)
        {
            _token = token;
            _name = name;
            _connection = connection;
        }

        public async Task HandlePushEvent(PushWebhookPayload payload, string directory)
        {
            if (!payload.Ref.Equals($"refs/heads/{payload.Repository.DefaultBranch}"))
            {
                return;
            }

            var app = new GitHubClient(new ProductHeaderValue(_name))
            {
                Credentials = new Octokit.Credentials(_token.Value(), AuthenticationType.Bearer)
            };

            var response = await app.GitHubApps.CreateInstallationToken(payload.Installation.Id);

            var installation = new GitHubClient(new ProductHeaderValue(_name))
            {
                Credentials = new Octokit.Credentials(response.Token, AuthenticationType.Oauth)
            };

            var clone = payload.Repository.CloneUrl;

            var source = clone.Insert(clone.IndexOf("://", StringComparison.Ordinal) + "://".Length,
                $"x-access-token:{response.Token}@");

            var path = Path.Combine(directory, "repository");

            if (Directory.Exists(path))
            {
                DirectoryHelper.DeleteDirectory(path);
            }

            global::LibGit2Sharp.Repository.Clone(source, path,
                new CloneOptions
                {
                    IsBare = true,
                    BranchName = payload.Repository.DefaultBranch,
                    RecurseSubmodules = false
                });

            var repository = new LibGit2Repository(path, clone);

            var modifications = new MatchesTransaction(_connection, new EloFormula());

            foreach (var commit in payload.Commits)
            {
                // TODO: Multiple commits in a row from an author must be squashed into one
                // to avoid add-delete-add-line attack to farm free reward.

                string report;

                try
                {
                    modifications.Start();

                    await repository.WriteInto(modifications, commit.Id);

                    modifications.Commit();

                    report = modifications.ToString()!;
                }
                catch (Exception exception)
                {
                    report = exception.ToString();
                }

                await installation.Repository.Comment.Create(payload.Repository.Id, commit.Id,
                    new NewCommitComment(report));
            }

            repository.Dispose();

            DirectoryHelper.DeleteDirectory(path);
        }
    }
}