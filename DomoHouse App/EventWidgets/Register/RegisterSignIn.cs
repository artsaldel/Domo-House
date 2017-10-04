
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Mica.Droid.EventWidgets.Menu;

namespace Mica.Droid.EventWidgets.Register
{
    [Activity(Label = "Register Layout", ScreenOrientation = ScreenOrientation.Portrait)]
    class RegisterSignIn : Activity
    {
        //All buttons
        private Button btnSignInFacebook;
        private Button btnSignInGmail;
        private Button btnEmailRegister;

        //The sign in progress bar
        private ProgressBar progressBarSignIn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Register);

            //Initializing all button
            btnSignInFacebook = FindViewById<Button>(Resource.Id.btnSignInFacebook);
            btnSignInGmail = FindViewById<Button>(Resource.Id.btnSignInGmail);
            btnEmailRegister = FindViewById<Button>(Resource.Id.btnEmailRegister);

            //Initializing the progress bar
            progressBarSignIn = FindViewById<ProgressBar>(Resource.Id.progressBarSignIn);

            /****************************************************/
            /********** Here all the button events **************/
            /****************************************************/
            //Click that sign the user using facebook
            btnSignInFacebook.Click += delegate
                {
                    EnterApp();
                };
            //Click that sign the user using gmail
            btnSignInGmail.Click += delegate
                {
                    EnterApp();
                };
            //Click that log the user using another email
            btnEmailRegister.Click += delegate 
                {
                    var intent = new Intent(this, typeof(RegisterEmail));
                    StartActivity(intent);
                };
            /****************************************************/

            //The progress bar becomes invisible
            progressBarSignIn.Visibility = ViewStates.Invisible;
        }

        //Thread that start the progress bar
        private void EnterApp()
        {
            progressBarSignIn.Visibility = ViewStates.Visible;
            Thread thread = new Thread(EnterAppAux);
            thread.Start();
        }

        //The progress starts
        private void EnterAppAux()
        {
            //Progress bar animation
            Thread.Sleep(5000);
            RunOnUiThread(() => { progressBarSignIn.Visibility = ViewStates.Invisible; });

            //Go to the init menu
            var intent = new Intent(this, typeof(MenuSlideTab));
            StartActivity(intent);
        }
    }
}