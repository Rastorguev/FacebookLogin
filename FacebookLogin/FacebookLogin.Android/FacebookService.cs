using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using FacebookLogin.Droid;
using Org.Json;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Forms;
using Object = Java.Lang.Object;

[assembly: Dependency(typeof(FacebookService))]

namespace FacebookLogin.Droid
{
    public class FacebookService : Object, IFacebookService, GraphRequest.IGraphJSONObjectCallback,
        GraphRequest.ICallback, IFacebookCallback
    {
        public static FacebookService Instance =>
            DependencyService.Get<IFacebookService>() as FacebookService;

        private readonly ICallbackManager _callbackManager = CallbackManagerFactory.Create();
        private readonly string[] _permissions = {@"public_profile", @"email"};

        private LoginResult _loginResult;
        private TaskCompletionSource<LoginResult> _completionSource;

        public FacebookService()
        {
            LoginManager.Instance.RegisterCallback(_callbackManager, this);
        }

        public Task<LoginResult> Login()
        {
            _completionSource = new TaskCompletionSource<LoginResult>();
            LoginManager.Instance.LogInWithReadPermissions(Forms.Context as Activity, _permissions);

            return _completionSource.Task;
        }

        public void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            _callbackManager?.OnActivityResult(requestCode, resultCode, data);
        }

        public void OnCompleted(JSONObject data, GraphResponse response)
        {
            OnCompleted(response);
        }

        public void OnCompleted(GraphResponse response)
        {
            if (response?.JSONObject == null)
            {
                _completionSource?.TrySetResult(new LoginResult(LoginState.Canceled, null));
            }
            else
            {
                var profile = new UserProfile(
                    Profile.CurrentProfile.FirstName,
                    Profile.CurrentProfile.LastName,
                    response.JSONObject.Has("email") ? response.JSONObject.GetString("email") : string.Empty,
                    response.JSONObject.GetJSONObject("picture")?.GetJSONObject("data")?.GetString("url")
                );

                _loginResult = new LoginResult(
                    LoginState.Success, profile
                );
            }

            _completionSource?.TrySetResult(_loginResult);
        }

        public void OnCancel()
        {
            _completionSource?.TrySetResult(new LoginResult(LoginState.Canceled, null));
        }

        public void OnError(FacebookException exception)
        {
            _completionSource?.TrySetResult(new LoginResult(LoginState.Failed, null, exception?.Message));
        }

        public void OnSuccess(Object result)
        {
            var facebookLoginResult = result.JavaCast<Xamarin.Facebook.Login.LoginResult>();
            if (facebookLoginResult == null)
            {
                return;
            }

            var parameters = new Bundle();
            parameters.PutString("fields", "id,email,picture.type(large)");
            var request = GraphRequest.NewMeRequest(facebookLoginResult.AccessToken, this);
            request.Parameters = parameters;
            request.ExecuteAsync();
        }
    }
}