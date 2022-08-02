using devrating.entity;
using devrating.factory;
using Microsoft.Extensions.Logging;

namespace devrating.consoleapp;

public sealed class ConsoleApplication : Application
{
    private readonly Database _database;
    private readonly Formula _formula;
    private readonly ILoggerFactory _loggerFactory;

    public ConsoleApplication(ILoggerFactory loggerFactory, Database database, Formula formula)
    {
        _loggerFactory = loggerFactory;
        _database = database;
        _formula = formula;
    }

    public DateTimeOffset PeriodStart()
    {
        return DateTimeOffset.UtcNow.AddDays(-90);
    }

    public void Top(Output output, string organization, string repository)
    {
        _database.Instance().Connection().Open();

        using var transaction = _database.Instance().Connection().BeginTransaction();

        try
        {
            if (!_database.Instance().Present())
            {
                _database.Instance().Create();
            }

            output.WriteLine("Author | Rating | Minimal additions in PR");
            output.WriteLine("------ | ------ | -------------------------");

            foreach (
                var author in _database.Entities().Authors().GetOperation()
                    .Top(organization, repository, PeriodStart())
                )
            {
                var rating = _database.Entities().Ratings().GetOperation().RatingOf(author.Id()).Value();

                var additions = _formula.SuggestedAdditionsPerWork(rating);

                output.WriteLine(
                    $"<{author.Email()}> | {rating:F2} | {additions:N0}"
                );
            }
        }
        finally
        {
            transaction.Rollback();
            _database.Instance().Connection().Close();
        }
    }

    public void Save(Diff diff)
    {
        _database.Instance().Connection().Open();

        using var transaction = _database.Instance().Connection().BeginTransaction();

        try
        {
            if (!_database.Instance().Present())
            {
                _database.Instance().Create();
            }

            if (diff.PresentIn(_database.Entities().Works()))
            {
                throw new InvalidOperationException("The diff is already added.");
            }

            var authorFactory = new DefaultAuthorFactory(_database.Entities().Authors());
            var ratings = _database.Entities().Ratings();

            diff.NewWork(
                new DefaultFactories(
                    authorFactory,
                    new DefaultWorkFactory(
                        _database.Entities().Works(),
                        ratings,
                        authorFactory
                    ),
                    new DefaultRatingFactory(
                        _loggerFactory,
                        authorFactory,
                        ratings,
                        _formula
                    )
                )
            );

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();

            throw;
        }
        finally
        {
            _database.Instance().Connection().Close();
        }
    }

    public bool IsDiffPresent(Diff diff)
    {
        _database.Instance().Connection().Open();

        using var transaction = _database.Instance().Connection().BeginTransaction();

        try
        {
            if (!_database.Instance().Present())
            {
                _database.Instance().Create();
            }

            return diff.PresentIn(_database.Entities().Works());
        }
        finally
        {
            transaction.Rollback();
            _database.Instance().Connection().Close();
        }
    }

    public void Print(Output output, Diff diff)
    {
        _database.Instance().Connection().Open();

        using var transaction = _database.Instance().Connection().BeginTransaction();

        try
        {
            if (!_database.Instance().Present())
            {
                _database.Instance().Create();
            }

            if (!diff.PresentIn(_database.Entities().Works()))
            {
                throw new InvalidOperationException("The diff is not present in the database. " +
                    "To insert, run `devrating update`.");
            }

            PrintWorkToOutput(output, diff.RelatedWork(_database.Entities().Works()));
        }
        finally
        {
            transaction.Rollback();
            _database.Instance().Connection().Close();
        }
    }

    private void PrintWorkToOutput(Output output, Work work)
    {
        output.WriteLine($"Author: <{work.Author().Email()}>");
        output.WriteLine($"Commit: {work.Commit()}");

        if (work.Since() is object)
        {
            output.WriteLine($"Since: {work.Since()}");
        }

        if (work.Link() is object)
        {
            output.WriteLine($"Link: {work.Link()}");
        }

        PrintWorkRatingsToOutput(output, work);
    }

    private void PrintWorkRatingsToOutput(Output output, Work work)
    {
        output.WriteLine("Author | Prev rating | New rating");
        output.WriteLine("------ | ----------- | ----------");

        foreach (var rating in _database.Entities().Ratings().GetOperation().RatingsOf(work.Id()).Reverse().ToList())
        {
            var previous = rating.PreviousRating();

            var before = previous.Id().Filled()
                ? previous.Value()
                : _formula.DefaultRating();

            output.WriteLine($"<{rating.Author().Email()}> | {before:F2} | {rating.Value():F2}");
        }
    }
}
