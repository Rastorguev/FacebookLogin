using System;
using Xamarin.Forms;

namespace FacebookLogin.Views
{
    //There is no need to use MVVM and bindings for such simple app.
    public partial class LoginPage
    {
        private readonly string UnknownErrorMessage = "Oops)";

        public LoginPage()
        {
            InitializeComponent();

            //There is no need to introduce localization approach
            Title = "Login";
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            //Service locator instead of dependency injection to simplify code
            var facebookService = DependencyService.Get<IFacebookService>();

            try
            {
                var loginResult = await facebookService.Login();
                if (loginResult.State == LoginState.Success && loginResult.Profile != null)
                {
                    await Navigation.PushAsync(new ProfilePage(loginResult.Profile));
                }
                else if (loginResult.State == LoginState.Failed)
                {
                    ShowError(loginResult.ErrorString ?? UnknownErrorMessage);
                }
            }
            catch (Exception)
            {
                ShowError(UnknownErrorMessage);
            }
        }

        private void ShowError(string errorMessage)
        {
            DisplayAlert("Error", errorMessage, "OK");
        }
    }
}