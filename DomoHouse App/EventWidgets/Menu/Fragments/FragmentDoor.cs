
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Mica.Droid.Server;
using System.Threading.Tasks;

namespace Mica.Droid.EventWidgets.Menu.Fragments
{
    class FragmentDoor : Fragment
    {
        //Toggle buttons definition
        ToggleButton btnDoor1;
        ToggleButton btnDoor2;
        ToggleButton btnDoor3;
        ToggleButton btnDoor4;

        //Text view definition
        TextView txtPuertaError;

        //Web client
        Client client;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.FragmentDoor, container, false);

            //Toogle buttons assigns
            btnDoor1 = view.FindViewById<ToggleButton>(Resource.Id.btnDoor1);
            btnDoor2 = view.FindViewById<ToggleButton>(Resource.Id.btnDoor2);
            btnDoor3 = view.FindViewById<ToggleButton>(Resource.Id.btnDoor3);
            btnDoor4 = view.FindViewById<ToggleButton>(Resource.Id.btnDoor4);

            //Text view assign
            txtPuertaError = view.FindViewById<TextView>(Resource.Id.txtPuertaError);

            //Se deshabilitan todos los botones
            btnDoor1.Enabled = false;
            btnDoor2.Enabled = false;
            btnDoor3.Enabled = false;
            btnDoor4.Enabled = false;

            //Set the web client
            client = new Client();

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();
            Task.Run(async () => {
                await UpdateDoors();
            });
        }

        async Task UpdateDoors()
        {
            while (true)
            {
                try
                {
                    string infoDoors = client.GetTextFromUrl();
                    string[] words = infoDoors.Split('.');

                    Activity.RunOnUiThread((() => btnDoor1.Checked = words[5].Equals("true")));
                    Activity.RunOnUiThread((() => btnDoor2.Checked = words[6].Equals("true")));
                    Activity.RunOnUiThread((() => btnDoor3.Checked = words[7].Equals("true")));
                    Activity.RunOnUiThread((() => btnDoor4.Checked = words[8].Equals("true")));

                    if (infoDoors == null)
                        Activity.RunOnUiThread((() => txtPuertaError.Text = "No se puede conectar con el servidor"));
                    else
                        Activity.RunOnUiThread((() => txtPuertaError.Text = null));
                }
                catch (System.Exception)
                {
                    Activity.RunOnUiThread((() => txtPuertaError.Text = "No se puede conectar con el servidor"));
                }
                await Task.Delay(3000);
            }
        }
    }
}