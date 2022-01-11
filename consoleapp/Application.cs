using devrating.factory;

namespace devrating.consoleapp;

public interface Application
{
    void Top(Log output, string organization, string repository);
    void Save(Diff diff);
    void PrintTo(Log output, Diff diff);
}
