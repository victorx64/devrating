using devrating.git.fake;
using Xunit;

namespace devrating.git.test;

public sealed class GitProcessFirstMergeCommitTest
{
    // $ git log --oneline --graph
    // * e3fb255 (HEAD -> d1) F
    // * 2fdd78c (main) E
    // *   03bf9c4 Merge branch 'c1'
    // |\
    // | * 7ae7eed C
    // * | 070e096 D
    // |/
    // * d72be68 B
    // * ec8cf81 A
    // * 839f05e I

    [Fact]
    public void ReturnsAfterCommitWhenItIsDestination()
    {
        Assert.Equal(
            "070e096b5ef09249bfca0dd3b379f5c052145c13",
            new GitProcessFirstMergeCommit(
                new FakeProcess("070e096b5ef09249bfca0dd3b379f5c052145c13"),
                new FakeProcess(""),
                new FakeProcess("")
            ).Sha()
        );
    }

    [Fact]
    public void ReturnsAfterCommitWhenItIsInDestinationBranch()
    {
        Assert.Equal(
            "070e096b5ef09249bfca0dd3b379f5c052145c13",
            new GitProcessFirstMergeCommit(
                new FakeProcess("070e096b5ef09249bfca0dd3b379f5c052145c13"),
                new FakeProcess(
@"2fdd78c92177aaa0e69a8f603cb853eaac7f487a
03bf9c4fa0066b418033e71258b8d625d7929f55"
                ),
                new FakeProcess(
@"2fdd78c92177aaa0e69a8f603cb853eaac7f487a
03bf9c4fa0066b418033e71258b8d625d7929f55"
                )
            ).Sha()
        );
    }

    [Fact]
    public void ReturnsAfterCommitWhenItIsInitialCommit()
    {
        Assert.Equal(
            "839f05e570a13a8147256b82deda91743c36d225",
            new GitProcessFirstMergeCommit(
                new FakeProcess("839f05e570a13a8147256b82deda91743c36d225"),
                new FakeProcess(
@"2fdd78c92177aaa0e69a8f603cb853eaac7f487a
03bf9c4fa0066b418033e71258b8d625d7929f55
070e096b5ef09249bfca0dd3b379f5c052145c13
d72be68e43aa549620f489f164a1075aa643a086
ec8cf8147aa360b647c1da75dcb5b0bdab08ad43"
                ),
                new FakeProcess(
@"2fdd78c92177aaa0e69a8f603cb853eaac7f487a
03bf9c4fa0066b418033e71258b8d625d7929f55
070e096b5ef09249bfca0dd3b379f5c052145c13
d72be68e43aa549620f489f164a1075aa643a086
ec8cf8147aa360b647c1da75dcb5b0bdab08ad43"
                )
            ).Sha()
        );
    }

    [Fact]
    public void ReturnsAfterCommitWhenItIsMergeCommit()
    {
        Assert.Equal(
            "03bf9c4fa0066b418033e71258b8d625d7929f55",
            new GitProcessFirstMergeCommit(
                new FakeProcess("03bf9c4fa0066b418033e71258b8d625d7929f55"),
                new FakeProcess("2fdd78c92177aaa0e69a8f603cb853eaac7f487a"),
                new FakeProcess("2fdd78c92177aaa0e69a8f603cb853eaac7f487a")
            ).Sha()
        );
    }

    [Fact]
    public void ReturnsMergeCommitWhenAfterCommitIsInAnotherBranch()
    {
        Assert.Equal(
            "03bf9c4fa0066b418033e71258b8d625d7929f55",
            new GitProcessFirstMergeCommit(
                new FakeProcess("03bf9c4fa0066b418033e71258b8d625d7929f55"),
                new FakeProcess(
@"2fdd78c92177aaa0e69a8f603cb853eaac7f487a
03bf9c4fa0066b418033e71258b8d625d7929f55
070e096b5ef09249bfca0dd3b379f5c052145c13"
                ),
                new FakeProcess(
@"2fdd78c92177aaa0e69a8f603cb853eaac7f487a
03bf9c4fa0066b418033e71258b8d625d7929f55"
                )
            ).Sha()
        );
    }

    [Fact]
    public void ReturnsAfterCommitWhenItIsNotReachableFromDestination()
    {
        Assert.Equal(
            "e3fb25511d944b94a83202533f8ca9efbd8e5f19",
            new GitProcessFirstMergeCommit(
                new FakeProcess("e3fb25511d944b94a83202533f8ca9efbd8e5f19"),
                new FakeProcess(""),
                new FakeProcess("")
            ).Sha()
        );
    }
}