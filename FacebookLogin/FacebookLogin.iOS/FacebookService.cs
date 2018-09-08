using System.Threading.Tasks;
using CoreGraphics;
using Facebook.CoreKit;
using Facebook.LoginKit;
using FacebookLogin.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(FacebookService))]

namespace FacebookLogin.iOS
{
    public class FacebookService : IFacebookService
    {
        private readonly LoginManager _loginManager = new LoginManager();
        private readonly string[] _permissions = {@"public_profile"};
        private TaskCompletionSource<LoginResult> _completionSource;

        public Task<LoginResult> Login()
        {
            _completionSource = new TaskCompletionSource<LoginResult>();
            _loginManager.LogInWithReadPermissions(_permissions, GetCurrentViewController(), LoginManagerLoginHandler);
            return _completionSource.Task;
        }

        private void LoginManagerLoginHandler(LoginManagerLoginResult result, NSError error)
        {
            if (result.IsCancelled)
            {
                _completionSource.TrySetResult(new LoginResult(LoginState.Canceled, null));
            }
            else if (error != null)
            {
                _completionSource.TrySetResult(new LoginResult
                (
                    LoginState.Failed,
                    null,
                    error.LocalizedDescription
                ));
            }
            else
            {
                var request = new GraphRequest(@"me", null);
                request.Start(GetDetailsRequestHandler);
            }
        }

        private void GetDetailsRequestHandler(GraphRequestConnection connection, NSObject result, NSError error)
        {
            if (error != null)
            {
                _completionSource.TrySetResult(new LoginResult
                (
                    LoginState.Failed,
                    null,
                    error.LocalizedDescription
                ));
            }
            else
            {
                var userProfile = new UserProfile(
                    Profile.CurrentProfile.FirstName,
                    Profile.CurrentProfile.LastName,
                    Profile.CurrentProfile.ImageUrl(ProfilePictureMode.Square, new CGSize(200, 200)).ToString()
                );

                _completionSource.TrySetResult(new LoginResult(LoginState.Success, userProfile));
            }
        }

        private static UIViewController GetCurrentViewController()
        {
            var viewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            while (viewController.PresentedViewController != null)
            {
                viewController = viewController.PresentedViewController;
            }

            return viewController;
        }
    }
}