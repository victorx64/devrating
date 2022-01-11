using devrating.factory;

namespace devrating.consoleapp;

public interface Application
{
    void Top(Output output, string organization, string repository);
    void Save(Diff diff);
    void PrintTo(Output output, Diff diff);
}
