
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
    [Activity(Label = "Register Photo Layout", ScreenOrientation = ScreenOrientation.Portrait)]
    class RegisterPhoto : Activity
    {
        //All buttons
        private Button btnRegisterSelectPhoto;
        private Button btnFinishRegisterPhoto;

        //The sign in progress bar
        private ProgressBar progressBarEmailSignIn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.RegisterPhoto);

            //Initializing all buttons
            btnRegisterSelectPhoto = FindViewById<Button>(Resource.Id.btnRegisterSelectPhoto);
            btnFinishRegisterPhoto = FindViewById<Button>(Resource.Id.btnFinishRegisterPhoto);

            //Initializing the progress bar
            progressBarEmailSignIn = FindViewById<ProgressBar>(Resource.Id.progressBarEmailSignIn);

            /****************************************************/
            /********** Here all the button events **************/
            /****************************************************/
            //Click that selects the photo
            btnRegisterSelectPhoto.Click += delegate
            {
                
            };
            //Click that finish the sign in
            btnFinishRegisterPhoto.Click += delegate
            {
                EnterApp();
            };
            /****************************************************/

            //The progress bar becomes invisible
            progressBarEmailSignIn.Visibility = ViewStates.Invisible;
        }

        //Thread that start the progress bar
        private void EnterApp()
        {
            progressBarEmailSignIn.Visibility = ViewStates.Visible;
            Thread thread = new Thread(EnterAppAux);
            thread.Start();
        }

        //The progress starts
        private void EnterAppAux()
        {
            //Animation for progress bar
            Thread.Sleep(5000);
            RunOnUiThread(() => { progressBarEmailSignIn.Visibility = ViewStates.Invisible; });

            //Go to the init menu
            var intent = new Intent(this, typeof(MenuSlideTab));
            StartActivity(intent);
        }
    }
}