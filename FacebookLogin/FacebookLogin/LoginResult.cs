namespace FacebookLogin
{
    public class LoginResult
    {
        public LoginState State { get; }
        public UserProfile Profile { get; }
        public string ErrorString { get; }

        public LoginResult(LoginState loginState, UserProfile profile, string errorString = null)
        {
            State = loginState;
            Profile = profile;
            ErrorString = errorString;
        }
    }
}