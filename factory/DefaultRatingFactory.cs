using devrating.entity;
using Microsoft.Extensions.Logging;

namespace devrating.factory;

public sealed class DefaultRatingFactory : RatingFactory
{
    private readonly AuthorFactory _authorFactory;
    private readonly Ratings _ratings;
    private readonly Formula _formula;
    private readonly ILogger<DefaultRatingFactory> _log;

    public DefaultRatingFactory(ILoggerFactory loggerFactory, AuthorFactory authorFactory, Ratings ratings, Formula formula)
    {
        _log = loggerFactory.CreateLogger<DefaultRatingFactory>();
        _authorFactory = authorFactory;
        _ratings = ratings;
        _formula = formula;
    }

    public IEnumerable<Rating> NewRatings(
        string organization,
        string repository,
        string email,
        IEnumerable<ContemporaryLines> deletions,
        Id work,
        DateTimeOffset createdAt
    )
    {
        var deletor = _authorFactory.AuthorAtOrg(organization, repository, email, createdAt);

        var current = _ratings.GetOperation().RatingOf(deletor);

        var value = current.Id().Filled()
            ? current.Value()
            : _formula.DefaultRating();

        bool NonSelfDeletion(ContemporaryLines lines)
        {
            return !lines.VictimEmail().Equals(email, StringComparison.OrdinalIgnoreCase);
        }

        deletions = deletions.Where(NonSelfDeletion).ToList();

        var ratings = new Dictionary<Id, (Rating, double)>();

        foreach (var deletion in deletions)
        {
            if (deletion.Weight() == 0d)
            {
                _log.LogInformation(new EventId(1518575), $"{email} ({value:F2}) deleted {deletion.Size()} lines with no rating change");
                continue;
            }

            var victim = _authorFactory.AuthorAtOrg(
                organization,
                repository,
                deletion.VictimEmail(),
                createdAt
            );

            var victimCurrent = _ratings.GetOperation().RatingOf(victim);

            var victimValue = victimCurrent.Id().Filled()
                ? victimCurrent.Value()
                : _formula.DefaultRating();

            var change = _formula.WinnerRatingChange(value, victimValue) * deletion.Weight();

            AddChange(ratings, deletor, current, change);
            AddChange(ratings, victim, victimCurrent, -change);

            _log.LogInformation(new EventId(1936766), $"{email} ({value:F2}) deleted {deletion.Size()} lines of {deletion.VictimEmail()} ({victimValue:F2}). Weight: {deletion.Weight():F2}. Rating change: {change:F2}");
        }

        return ratings.Select(
            r => _ratings.InsertOperation().Insert(
                (
                    r.Value.Item1.Id().Filled()
                        ? r.Value.Item1.Value()
                        : _formula.DefaultRating()
                ) + r.Value.Item2,
                r.Value.Item1.Id(),
                work,
                r.Key
            )
        )
        .ToList();
    }

    private void AddChange(
        Dictionary<Id, (Rating, double)> ratings,
        Id author,
        Rating current,
        double change
    )
    {
        if (ratings.ContainsKey(author))
        {
            ratings[author] = (current, ratings[author].Item2 + change);
        }
        else
        {
            ratings[author] = (current, change);
        }
    }
}
