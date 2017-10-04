
using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Mica.Droid.EventWidgets.Menu.Fragments.FragmentsMe
{
    public class FragmentEditProfile : Fragment
    {
        //All buttons
        private Button btnReadyEditProfile;
        private ImageButton btnImageEditProfile;

        //All edit text
        private EditText txtNameEditProfile;
        private EditText txtEmailEditProfile;

        //Back handler button

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.FragmentEditProfile, container, false);

            //Initializing all button
            btnReadyEditProfile = view.FindViewById<Button>(Resource.Id.btnReadyEditProfile);
            btnImageEditProfile = view.FindViewById<ImageButton>(Resource.Id.btnImageEditProfile);

            //Initialing all edit text
            txtNameEditProfile = view.FindViewById<EditText>(Resource.Id.txtNameEditProfile);
            txtEmailEditProfile = view.FindViewById<EditText>(Resource.Id.txtEmailEditProfile);

            //Setting events on click
            btnReadyEditProfile.Click += ReadyEditProfileClick;
            btnImageEditProfile.Click += ImageEditProfileClick;

            return view;
        }

        //Click for edit profile ready
        void ReadyEditProfileClick(object sender, EventArgs e)
        {

        }

        //Click for select new profile image
        void ImageEditProfileClick(object sender, EventArgs e)
        {

        }
    }
}