using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DevRating.Domain.Git;
using DevRating.GitHubApp.Models;
using DevRating.LibGit2Sharp;
using LibGit2Sharp;
using Octokit;

namespace DevRating.GitHubApp
{
    public sealed class Application
    {
        private readonly JsonWebToken _token;
        private readonly string _name;
        private readonly ModificationsStorage _storage;

        public Application(JsonWebToken token, string name, ModificationsStorage storage)
        {
            _token = token;
            _name = name;
            _storage = storage;
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

            try
            {
                var clone = payload.Repository.CloneUrl;

                var source = clone.Insert(clone.IndexOf("://", StringComparison.Ordinal) + "://".Length,
                    $"x-access-token:{response.Token}@");

                var path = Path.Combine(directory, "repository");

                if (Directory.Exists(path))
                {
                    DirectoryHelper.DeleteDirectory(path);
                }

                global::LibGit2Sharp.Repository.Clone(source, path, new CloneOptions
                {
                    IsBare = true,
                    BranchName = payload.Repository.DefaultBranch,
                    RecurseSubmodules = false
                });

                var repository = new LibGit2Repository(path, clone);

                var modifications = new DefaultModificationsCollection();

                foreach (var commit in payload.Commits)
                {
                    await repository.WriteInto(modifications, commit.Id);
                }

                await installation.Repository.Comment.Create(
                    payload.Repository.Id,
                    payload.Commits.Last().Id,
                    new NewCommitComment(modifications.PutTo(_storage)));

                repository.Dispose();

                DirectoryHelper.DeleteDirectory(path);
            }
            catch (Exception exception)
            {
                await installation.Repository.Comment.Create(
                    payload.Repository.Id,
                    payload.Commits.Last().Id,
                    new NewCommitComment(exception.Message));
            }
        }
    }
}