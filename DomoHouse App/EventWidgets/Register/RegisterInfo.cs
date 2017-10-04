
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Java.Lang;
using Mica.Droid.EventWidgets.Menu;
using Mica.Droid.Security;
using Mica.Droid.DataAccess;

namespace Mica.Droid.EventWidgets.Register
{
    [Activity(Label = "Register Info Layout", ScreenOrientation = ScreenOrientation.Portrait)]
    class RegisterInfo : Activity
    {
        //All buttons
        private Button btnNextRegisterInfo;

        //All edit text
        private EditText txtNameRegister;
        private EditText txtPassRegister;

        //Text view
        TextView txtInfoError;

        //The sign in progress bar
        private ProgressBar progressBarEmailSignIn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.RegisterInfo);

            //Initializing all buttons
            btnNextRegisterInfo = FindViewById<Button>(Resource.Id.btnNextRegisterInfo);

            //Initializing all edit text
            txtNameRegister = FindViewById<EditText>(Resource.Id.txtNameRegister);
            txtPassRegister = FindViewById<EditText>(Resource.Id.txtPassRegister);

            //Text view
            txtInfoError = FindViewById<TextView>(Resource.Id.txtInfoError);

            //Initializing the progress bar
            progressBarEmailSignIn = FindViewById<ProgressBar>(Resource.Id.progressBarEmailSignIn2);

            /****************************************************/
            /********** Here all the button events **************/
            /****************************************************/
            //Click for enter app
            btnNextRegisterInfo.Click += delegate
            {
                VerifyInfo();
            };
            /****************************************************/

            //The progress bar becomes invisible
            progressBarEmailSignIn.Visibility = ViewStates.Invisible;
        }

        public async void VerifyInfo()
        {
            string name = txtNameRegister.Text;
            string pass = txtPassRegister.Text;
            bool nameCondition = string.IsNullOrWhiteSpace(name);
            bool passCondition = string.IsNullOrWhiteSpace(pass);
            if (nameCondition && passCondition)
                txtInfoError.Text = "Ingrese su información";
            else if (nameCondition)
                txtInfoError.Text = "Ingrese su nombre";
            else if (passCondition)
                txtInfoError.Text = "Ingrese su contraseña";
            else
            {
                txtInfoError.Text = null;
                MainActivity.userOnline.Name = name;
                MainActivity.userOnline.Password = Encrypt.EncryptString(pass);
                MainActivity.userOnline.IsOnApp = true;
                DBAccess newDB = new DBAccess();
                await newDB.AddNewUserAsync(MainActivity.userOnline);
                EnterApp();
            }
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