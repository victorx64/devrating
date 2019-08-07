namespace DevRating.Rating
{
    public interface Output
    {
        void Write(string line);
        void WriteLine(string line);
        void WriteVerbose(string line);
        void WriteVerboseLine(string line);
    }
}