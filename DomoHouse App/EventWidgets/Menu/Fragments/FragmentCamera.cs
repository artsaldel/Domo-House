
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using Mica.Droid.Server;


namespace Mica.Droid.EventWidgets.Menu.Fragments
{
    class FragmentCamera : Fragment
    {
        private Button btnNewPhoto;
        private ImageView imgCamera;
        private TextView txtServidorError;

        Client client;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.FragmentCamera, container, false);

            //Initializing all the button
            btnNewPhoto = view.FindViewById<Button>(Resource.Id.btnNewPhoto);

            //Initializing the camera image
            imgCamera = view.FindViewById<ImageView>(Resource.Id.imageViewCamera);

            //Initializing Text views
            txtServidorError = view.FindViewById<TextView>(Resource.Id.txtServidorError);

            //Setting the events on click
            btnNewPhoto.Click += NewPhotoClick;

            //Setting the image
            GetPhoto();

            //Set the web client
            client = new Client();

            return view;
        }

        //Show the edit profile fragment
        void NewPhotoClick(object sender, EventArgs e)
        {
            GetPhoto();
        }

        private void GetPhoto()
        {
            try
            {
                var imageBitmap = client.GetImageBitmapFromUrl();
                imgCamera.SetImageBitmap(imageBitmap);
                txtServidorError.Text = null;
            }
            catch (Exception e)
            {
                txtServidorError.Text = "No se puede conectar con el servidor";
            }
        }
    }
}