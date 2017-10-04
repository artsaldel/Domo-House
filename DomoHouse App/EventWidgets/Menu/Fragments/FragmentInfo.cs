
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Mica.Droid.EventWidgets.Menu.Fragments.FragmentsMe;
using System;
using Mica.Droid.DataAccess;


using Android.Content;


namespace Mica.Droid.EventWidgets.Menu.Fragments
{
    class FragmentInfo : Fragment
    {
        private Button btnAbout;
        private Button btnClose;
        private Button btnDeleteDB2;

        private TextView txtMyInfo;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.FragmentInfo, container, false);

            //Initializing all the button
            btnAbout = view.FindViewById<Button>(Resource.Id.btnAbout);
            btnClose = view.FindViewById<Button>(Resource.Id.btnCloseApp);
            btnDeleteDB2 = view.FindViewById<Button>(Resource.Id.btnDeleteDB2);

            //Text views
            txtMyInfo = view.FindViewById<TextView>(Resource.Id.txtMyInfo);
            txtMyInfo.Text = MainActivity.userOnline.Name + " - " + MainActivity.userOnline.Email;

            //Setting the events on click
            btnAbout.Click += AboutClick;
            btnClose.Click += CloseClick;
            btnDeleteDB2.Click += DeleteDB;

            return view;
        }

        //Show the configuration fragment
        void AboutClick(object sender, EventArgs e)
        {
            Fragment fragment = new FragmentAbout { Arguments = new Bundle() };
            ShowFragment(fragment);
        }

        async void CloseClick(object sender, EventArgs e)
        {
            DBAccess newDB = new DBAccess();
            MainActivity.userOnline.IsOnApp = false;
            await newDB.UpdateUserAsync(MainActivity.userOnline);

            //Go to the main menu
            StartMainActivity();
        }

        async void DeleteDB(object sender, EventArgs e)
        {
            DBAccess newDB = new DBAccess();
            await newDB.DeleteAllAsync();

            //Go to the main menu
            StartMainActivity();
        }

        //Show the passing fragment
        private void ShowFragment(Fragment fragment)
        {
            var ft = FragmentManager.BeginTransaction();
            ft.AddToBackStack("Fragment");
            ft.Replace(Resource.Id.container, fragment);
            ft.Commit();
        }

        private void StartMainActivity()
        {
            this.Activity.Finish();
            Intent intent = new Intent(this.Activity, typeof(MainActivity));
            StartActivity(intent); 
        }
    }
}