
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Mica.Droid.EventWidgets.Register;
using Mica.Droid.EventWidgets.Menu;
using Android.Content.PM;
using Mica.Droid.DataAccess;
using Mica.Droid.Security;
using System.Collections.Generic;

namespace Mica.Droid
{
    [Activity(MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        //Usuario en linea
        public static Usuario userOnline = null;

        //All buttons
        private Button btnLogIn;
        private Button btnRegister;

        //All edit text
        private EditText txtEmailLogin;
        private EditText txtPassLogin;

        //Textiew
        TextView txtPrueba;

        //The login progress bar
        private ProgressBar progressBarLogIn;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.NoTitle);

            //Verify if there is any user online
            VerifyUserOnline();


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //Initializing all buttons
            btnRegister = FindViewById<Button>(Resource.Id.btnRegister);
            btnLogIn = FindViewById<Button>(Resource.Id.btnLogIn);

            //Initializing all edit text
            txtEmailLogin = FindViewById<EditText>(Resource.Id.txtEmailLogin);
            txtPassLogin = FindViewById<EditText>(Resource.Id.txtPassLogin);

            //Texview
            txtPrueba = FindViewById<TextView>(Resource.Id.txtPrueba);

            //Initializing the progress bar
            progressBarLogIn = FindViewById<ProgressBar>(Resource.Id.progressBarLogIn);
            progressBarLogIn.Visibility = ViewStates.Invisible;

            /****************************************************/
            /********** Here all the button events **************/
            /****************************************************/
            //Click that log the user
            btnLogIn.Click += delegate 
                {
                    VerifyUserEnter();
                };

            //Click that log the user using another email
            btnRegister.Click += delegate 
                {
                    userOnline = new Usuario();
                    var intent = new Intent(this, typeof(RegisterEmail));
                    StartActivity(intent);
                };
            /****************************************************/

            //Prueba
            //DBAccess db = new DBAccess();
            //txtPrueba.Text = FileAccessHelper.status;
        }

        private async void VerifyUserOnline()
        {
            DBAccess newDB = new DBAccess();
            userOnline = await newDB.GetUserOnline();
            if (userOnline != null)
            {
                var intent = new Intent(this, typeof(MenuSlideTab));
                StartActivity(intent);
            }
        }

        private async void VerifyUserEnter()
        {
            string emailEnter = txtEmailLogin.Text;
            string passEnter = txtPassLogin.Text;
            DBAccess newDB = new DBAccess();
            userOnline = await newDB.GetUser(emailEnter);
            if (string.IsNullOrWhiteSpace(emailEnter))
                txtPrueba.Text = "Ingrese su email";
            else if (string.IsNullOrWhiteSpace(passEnter))
                txtPrueba.Text = "Ingrese su contraseña";
            else if (userOnline != null)
            {
                string passEncrypted = userOnline.Password;
                bool passwordCondition = Encrypt.DecryptString(passEncrypted) == passEnter;
                if (passwordCondition)
                {
                    userOnline.IsOnApp = true;
                    await newDB.UpdateUserAsync(userOnline);
                    txtPrueba.Text = null;
                    EnterApp();
                }
                else
                    txtPrueba.Text = "La contraseña es incorrecta";
            }
            else
                txtPrueba.Text = "El usuario no existe";
        }

        //Thread that start the progress bar
        private void EnterApp()
        {
            progressBarLogIn.Visibility = ViewStates.Visible;
            Thread thread = new Thread(EnterAppAux);
            thread.Start();
        }

        //The progress starts
        private void EnterAppAux()
        {
            //Progress bar animation
            Thread.Sleep(5000);
            RunOnUiThread( () => { progressBarLogIn.Visibility = ViewStates.Invisible; } );

            //Go to the init menu
            var intent = new Intent(this, typeof(MenuSlideTab));
            StartActivity(intent);
        }

        //Disable the back pressed button
        public override void OnBackPressed() { }
    }
}