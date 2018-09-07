using System;
using Xamarin.Forms;

namespace FacebookLogin.Views
{
    public partial class LoginPage
    {
        private readonly string UnknownErrorMessage = "Oops)";

        public LoginPage()
        {
            InitializeComponent();

            Title = "Login";
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
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
            DisplayAlert("Error", errorMessage, "Ok");
        }
    }
}