
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Mica.Droid.EventWidgets.Menu.Fragments;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace Mica.Droid.EventWidgets.Menu
{
    [Activity(Theme = "@style/CustomActionBarTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    class MenuSlideTab : Activity
    {
        //All tab image buttons
        private ImageButton btnCamera;
        private ImageButton btnDoor;
        private ImageButton btnLight;
        private ImageButton btnHouse;
        private ImageButton btnInfo;

        //Some colors for the tab layout
        private Color selectedColor;
        private Color deselectedColor;
        private Color notificationColor;

        //All the fragments
        private Fragment currentFragment;
        private Fragment fragmentCamera;
        private Fragment fragmentDoor;
        private Fragment fragmentLight;
        private Fragment fragmentHouse;
        private Fragment fragmentInfo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.MenuSlideTab);

            //Initializing the buttons
            btnCamera = FindViewById<ImageButton>(Resource.Id.btnCamera);
            btnDoor = FindViewById<ImageButton>(Resource.Id.btnDoor);
            btnLight = FindViewById<ImageButton>(Resource.Id.btnLight);
            btnHouse = FindViewById<ImageButton>(Resource.Id.btnHouse);
            btnInfo = FindViewById<ImageButton>(Resource.Id.btnInfo);

            //Initializing selected colors
            #pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            selectedColor = Resources.GetColor(Resource.Color.grey);
            deselectedColor = Resources.GetColor(Resource.Color.white);
            notificationColor = Resources.GetColor(Resource.Color.red);
            #pragma warning restore CS0618 // El tipo o el miembro están obsoletos

            //Initilizing the fragments
            InitiateFragments();

            //Setting events on click
            btnCamera.Click += delegate {
                ShowTabCamera();
            };

            btnDoor.Click += delegate {
                ShowTabDoor();
            };

            btnLight.Click += delegate {
                ShowTabLight();
            };

            btnHouse.Click += delegate {
                ShowTabHouse();
            };

            btnInfo.Click += delegate {
                ShowTabInfo();
            };
        }

        //Initiating the fragments
        private void InitiateFragments()
        {
            //Initializing the fragments
            fragmentCamera = new FragmentCamera();
            fragmentDoor = new FragmentDoor();
            fragmentLight = new FragmentLight();
            fragmentHouse = new FragmentHouse();
            fragmentInfo = new FragmentInfo();

            //Setting the fragments
            var trans = FragmentManager.BeginTransaction();
            trans.Add(Resource.Id.container, fragmentCamera);
            trans.Hide(fragmentCamera);
            trans.Add(Resource.Id.container, fragmentDoor);
            trans.Hide(fragmentDoor);
            trans.Add(Resource.Id.container, fragmentHouse);
            trans.Hide(fragmentHouse);
            trans.Add(Resource.Id.container, fragmentInfo);
            trans.Hide(fragmentInfo);
            trans.Add(Resource.Id.container, fragmentLight);
            trans.Hide(fragmentLight);
            trans.Commit();
            currentFragment = fragmentDoor;
            ShowTabLight();
        }

        //Deselect all button image
        private void DeselectAll()
        {
            btnCamera.SetColorFilter(deselectedColor);
            btnDoor.SetColorFilter(deselectedColor);
            btnLight.SetColorFilter(deselectedColor);
            btnHouse.SetColorFilter(deselectedColor);
            btnInfo.SetColorFilter(deselectedColor);
        }

        //Show the passing fragment
        private void ShowFragment(Fragment fragment)
        {
            var trans = FragmentManager.BeginTransaction();
            trans.Hide(currentFragment);
            trans.Show(fragment);
            trans.AddToBackStack(null);
            trans.Commit();
            currentFragment = fragment;
        }

        /*************************************************************************************************
        ****************************************** SHOW THE TABS******************************************
        **************************************************************************************************/
        private void ShowTabCamera()
        {
            DeselectAll();
            btnCamera.SetColorFilter(selectedColor);
            ShowFragment(fragmentCamera);
        }

        private void ShowTabDoor()
        {
            DeselectAll();
            btnDoor.SetColorFilter(selectedColor);
            ShowFragment(fragmentDoor);
        }

        private void ShowTabLight()
        {
            DeselectAll();
            btnLight.SetColorFilter(selectedColor);
            ShowFragment(fragmentLight);
        }

        private void ShowTabHouse()
        {
            DeselectAll();
            btnHouse.SetColorFilter(selectedColor);
            ShowFragment(fragmentHouse);
        }

        private void ShowTabInfo()
        {
            DeselectAll();
            btnInfo.SetColorFilter(selectedColor);
            ShowFragment(fragmentInfo);
        }
        /*************************************************************************************************/
        /*************************************************************************************************/
    }
}