namespace FacebookLogin
{
    public class UserProfile
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string ImageUrl { get; }

        public UserProfile(string firstName, string lastName, string imageUrl)
        {
            FirstName = firstName;
            LastName = lastName;
            ImageUrl = imageUrl;
        }
    }
}