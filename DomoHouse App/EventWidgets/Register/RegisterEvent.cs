
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Mica.ViewModels;
using System;

namespace Mica.Droid.EventWidgets.Register
{
    class RegisterEvent : DialogFragment
    {
        private EditText txtNameSignIn;
        private EditText txtEmailSignIn;
        private EditText txtPasswordSignIn;
        private Button btnNextSignIn;

        public event EventHandler<RegisterViewModel> registerComplete;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.Register, container, false);
            /*
            var view = inflater.Inflate(Resource.Layout.DialogSignIn, container, false);
            txtNameSignIn = view.FindViewById<EditText>(Resource.Id.txtNameSignIn);
            txtEmailSignIn = view.FindViewById<EditText>(Resource.Id.txtEmailSignIn);
            txtPasswordSignIn = view.FindViewById<EditText>(Resource.Id.txtPasswordSignIn);
            btnNextSignIn = view.FindViewById<Button>(Resource.Id.btnNextSignIn);
            */
            btnNextSignIn.Click += btnNextSignIn_Click;

            return view;
        }
        void btnNextSignIn_Click(object sender, EventArgs e)
        {
            //User has clicked the sign in button
            string name = txtNameSignIn.Text;
            string email = txtEmailSignIn.Text;
            string pass = txtPasswordSignIn.Text;
            registerComplete.Invoke(this, new RegisterViewModel(name, email, pass));
            this.Dismiss();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle); //Set the title bar to invisible
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation; // Set the animation
        }
    }
}