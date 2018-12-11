namespace User.API.Models
{
    public class UserProperty
    {
        private int? _requestedHashCode;

        public int AppUserId { get; set; }

        public string Key { get; set; }
        public string Text { get; set; }

        public string Value { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as UserProperty;
            return other != null && GetType() == obj.GetType()
                                 && Key.Equals(other.Key)
                                 && Value.Equals(other.Value); 
        }

        public override int GetHashCode()
        {
            if (!_requestedHashCode.HasValue)
            {
                _requestedHashCode = (Key + Value).GetHashCode() ^ 31;  //XOR for random distribution
            }
            return _requestedHashCode.Value;
        }
    }
}