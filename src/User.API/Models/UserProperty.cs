namespace User.API.Models
{
    public class UserProperty
    {
        public int AppUserId { get; set; }

        public string Key { get; set; }
        public string Text { get; set; }

        public string Value { get; set; }
    }
}