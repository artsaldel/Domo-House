
using Android.App;
using Android.OS;
using Android.Views;

namespace Mica.Droid.EventWidgets.Menu.Fragments
{
    class FragmentHouse : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.FragmentHouse, container, false);
            return view;
        }
    }
}