namespace DevRating.Game
{
    public class PlayersPair
    {
        private readonly string _first;
        private readonly string _second;

        public PlayersPair(string first, string second)
        {
            _first = first;
            _second = second;
        }

        public string First()
        {
            return _first;
        }

        public string Second()
        {
            return _second;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PlayersPair) obj);
        }

        protected bool Equals(PlayersPair other)
        {
            return _first == other._first && _second == other._second;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_first.GetHashCode() * 397) ^ _second.GetHashCode();
            }
        }
    }
}