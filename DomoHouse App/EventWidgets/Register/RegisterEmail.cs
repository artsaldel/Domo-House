using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;

namespace Mica.Droid.EventWidgets.Register
{
    [Activity(Label = "Register Email Layout", ScreenOrientation = ScreenOrientation.Portrait)]
    class RegisterEmail : Activity
    {
        //All buttons
        private Button btnNextRegisterEmail;

        //All edit text
        private EditText txtEmailRegister;

        //Textviews
        private TextView txtEmailError;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.RegisterEmail);

            //Initializing all buttons
            btnNextRegisterEmail = FindViewById<Button>(Resource.Id.btnNextRegisterEmail);

            //Initializing all edit text
            txtEmailRegister = FindViewById<EditText>(Resource.Id.txtEmailRegister);

            //Text view
            txtEmailError = FindViewById<TextView>(Resource.Id.txtEmailError);

            /****************************************************/
            /********** Here all the button events **************/
            /****************************************************/
            //Click that permits the user to add the personal info
            btnNextRegisterEmail.Click += delegate
            {
                VerifyEmail();
            };
            /****************************************************/
        }

        public void VerifyEmail()
        {
            string email = txtEmailRegister.Text;
            if (string.IsNullOrWhiteSpace(email))
                txtEmailError.Text = "Ingrese un email";
            else
            {
                MainActivity.userOnline.Email = email;
                txtEmailError.Text = null;
                var intent = new Intent(this, typeof(RegisterInfo));
                StartActivity(intent);
            }
        }
    }
}