using System;
using Xamarin.Forms;

namespace FacebookLogin.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage(UserProfile profile)
        {
            InitializeComponent();

            Title = "Profile";

            if (!string.IsNullOrEmpty(profile.ImageUrl))
            {
                ProfileImage.Source = ImageSource.FromUri(new Uri(profile.ImageUrl));
            }
        }
    }
}