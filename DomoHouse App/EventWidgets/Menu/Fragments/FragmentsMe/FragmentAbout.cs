
using Android.App;
using Android.OS;
using Android.Views;

namespace Mica.Droid.EventWidgets.Menu.Fragments.FragmentsMe
{
    public class FragmentAbout : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.FragmentAbout, container, false);
            return view;
        }
    }
}