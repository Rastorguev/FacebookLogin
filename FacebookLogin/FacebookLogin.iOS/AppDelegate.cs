using Facebook.CoreKit;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace FacebookLogin.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        ////
        //// This method is invoked when the application has loaded and is ready to run. In this 
        //// method you should instantiate the window, load the UI into it and then make the window
        //// visible.
        ////
        //// You have 17 seconds to return from this method, or iOS will terminate your application.
        ////
        //public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        //{
        //    global::Xamarin.Forms.Forms.Init();
        //    LoadApplication(new App());

        //    return base.FinishedLaunching(app, options);
        //}

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            LoadApplication(new App());
            Profile.EnableUpdatesOnAccessTokenChange(true);
            ApplicationDelegate.SharedInstance.FinishedLaunching(app, options);

            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication,
            NSObject annotation)
        {
            return ApplicationDelegate.SharedInstance.OpenUrl(application, url, sourceApplication, annotation);
        }

        public override void OnActivated(UIApplication application)
        {
            AppEvents.ActivateApp();
        }
    }
}