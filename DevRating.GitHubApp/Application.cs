using System;
using System.Threading.Tasks;
using DevRating.Game;
using DevRating.Game.Test;
using DevRating.LibGit2Sharp;
using DevRating.Rating;
using LibGit2Sharp;
using Octokit;
using Repository = DevRating.Git.Repository;

namespace DevRating.GitHubApp
{
    public sealed class Application
    {
        private readonly JsonWebToken _token;
        private readonly string _name;

        public Application(JsonWebToken token, string name)
        {
            _token = token;
            _name = name;
        }

        public async Task HandlePushEvent(PushWebhookPayload payload)
        {
            /*
             * git clone bare repo
             * foreach payload.commit
             * try
                * analyze each commit
                * post report to comments 
             * catch
                * post error message
                * invite to retry or create issue ticket
             */

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

            var path = global::LibGit2Sharp.Repository.Clone(source, "repo.git", new CloneOptions
            {
                IsBare = true,
                BranchName = payload.Repository.DefaultBranch,
                RecurseSubmodules = false
            });

            var matches = new FakeMatches(1200d);

            Repository repository = new LibGit2Repository(path);

            foreach (var commit in payload.Commits)
            {
                var games = new Games(commit.Id, new EloFormula(), 2000d);

                await repository.WriteInto(games, commit.Id);

                await games.PushInto(matches);

                await installation.Repository.Comment.Create(payload.Repository.Id, commit.Id,
                    new NewCommitComment(matches.Report(commit.Id)));
            }
        }
    }
}