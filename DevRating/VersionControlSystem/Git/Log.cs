using System.Collections.Generic;

namespace DevRating.VersionControlSystem.Git
{
    public class Log
    {
        private readonly IProcess _process;

        public Log(IProcess process)
        {
            _process = process;
        }

        public IEnumerable<Diff> Diffs()
        {
            var stream = _process.Output("git",
                "--no-pager log --reverse -U0 -C --pretty=format:commit,%H,%aE --use-mailmap --simplify-merges --full-history --no-prefix");

            var filesAfter = new Dictionary<string, File>();
            var filesBefore = new Dictionary<string, File>();
            var diffs = new List<Diff>();

            Commit commit = null;
            var previous = string.Empty;
            var current = string.Empty;
            int i = 0;
            
            while (!stream.EndOfStream)
            {
                var line = stream.ReadLine();

                if (line.StartsWith("commit,"))
                {
                    ++i;
                    filesBefore = filesAfter;
                    filesAfter = new Dictionary<string, File>(filesBefore);
                    commit = new Commit(line);
                    if (commit.Hash() == "598374144f9a930b59db050126543a0609350520") 
                        ;
                }
                else if (line.StartsWith("--- "))
                {
                    previous = line.Substring("--- ".Length).TrimEnd('\t');
                }
                else if (line.StartsWith("rename from "))
                {
                    previous = line.Substring("rename from ".Length);
                }
                else if (line.StartsWith("copy from "))
                {
                    previous = line.Substring("copy from ".Length);
                }
                else if (line.StartsWith("+++ "))
                {
                    current = line.Substring("+++ ".Length).TrimEnd('\t');

                    if (current.Equals("/dev/null")) // file deleted
                    {
                        current = $"%{commit.Hash()}/{previous}";

                        RenameFile(filesBefore, filesAfter, previous, current, commit.Hash());
                    }
                    else
                    {
                        if (previous.Equals("/dev/null")) // file added
                        {
                            var file = new File(new string[0], commit.Hash(), current);

                            AddFile(filesAfter, current, file);
                        }
                        else if (previous.Equals(current))// file modified
                        {
                            RenameFile(filesBefore, filesAfter, previous, current, commit.Hash());
                        }
                        else
                        {
                            // file renamed or copied
                        }
                    }
                }
                else if (line.StartsWith("rename to "))
                {
                    current = line.Substring("rename to ".Length);

                    if (filesBefore.ContainsKey(previous))
                    {
                        RenameFile(filesBefore, filesAfter, previous, current, commit.Hash());
                    }
                    else
                    {
                        // renamed a binary file
                    }
                }
                else if (line.StartsWith("copy to "))
                {
                    current = line.Substring("copy to ".Length);
                    
                    if (commit.Hash() == "50a806f8ee54d8579a5d7bc2f70e79cce766332d" && current.Equals(
                            "src/mobile/LSP.Mobile.iOS/obj/iPhoneSimulator/Release/ibtool-manifests/MainStoryboard.storyboardc")
                    )
                        ;
                        
                    if (filesBefore.ContainsKey(previous))
                    {
                        
                        var file = new File(filesBefore[previous].Authors(), commit.Hash(), current);

                        AddFile(filesAfter, current, file);
                    }
                    else
                    {
                        // copied a binary file
                    }
                }
                else if (line.StartsWith("@@ "))
                {
                    var file = filesAfter[current];

                    var parts = line.Split(' ');

                    file.AddDeletion(new LinesBlock(commit.Author(), parts[1]));
                    file.AddAddition(new LinesBlock(commit.Author(), parts[2]));
                }
            }

            return diffs;
        }

        private void AddFile(IDictionary<string, File> files, string current, File file)
        {
            if (current == "src/mobile/LSP.Mobile.Business/obj/Release/build.force")
                ;
            
            files.Add(current, file);
        }
        
        private void RenameFile(IDictionary<string, File> filesBefore, IDictionary<string, File> filesAfter, string previous, string current, string commit)
        {
            var file = new File(filesBefore[previous].Authors(), commit, current);

            filesAfter.Remove(previous);

            AddFile(filesAfter, current, file);
        }
    }
}