namespace DevRating.Rating
{
    public interface IOutput
    {
        void Write(string line);
        void WriteLine(string line);
        void WriteVerbose(string line);
        void WriteVerboseLine(string line);
    }
}