using devrating.factory;

namespace devrating.consoleapp;

public interface Application
{
    DateTimeOffset PeriodStart();
    void Top(Output output, string organization, string repository);
    void Save(Diff diff);
    void Print(Output output, Diff diff);
    bool IsCommitPresent(string organization, string repository, string commit);
}
