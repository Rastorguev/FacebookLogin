namespace FacebookLogin
{
    public class UserProfile
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string ImageUrl { get; }

        public UserProfile(string firstName, string lastName, string email, string imageUrl)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            ImageUrl = imageUrl;
        }
    }
}