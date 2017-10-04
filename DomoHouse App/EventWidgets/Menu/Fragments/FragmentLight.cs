
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Mica.Droid.Server;
using System;

namespace Mica.Droid.EventWidgets.Menu.Fragments
{
    class FragmentLight : Fragment
    {
        //Togglebuttons
        ToggleButton btnLight1;
        ToggleButton btnLight2;
        ToggleButton btnLight3;
        ToggleButton btnLight4;
        ToggleButton btnLight5;

        //Buttons
        Button btnOffLights;
        Button btnOnLights;

        //Text view
        TextView txtLuzError;

        //Web client
        Client client;

        //String state of lights
        public static string stateLight1;
        public static string stateLight2;
        public static string stateLight3;
        public static string stateLight4;
        public static string stateLight5;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.FragmentLight, container, false);

            //Togglebuttons
            btnLight1 = view.FindViewById<ToggleButton>(Resource.Id.btnLight1);
            btnLight2 = view.FindViewById<ToggleButton>(Resource.Id.btnLight2);
            btnLight3 = view.FindViewById<ToggleButton>(Resource.Id.btnLight3);
            btnLight4 = view.FindViewById<ToggleButton>(Resource.Id.btnLight4);
            btnLight5 = view.FindViewById<ToggleButton>(Resource.Id.btnLight5);

            //Buttons
            btnOffLights = view.FindViewById<Button>(Resource.Id.btnOffLights);
            btnOnLights = view.FindViewById<Button>(Resource.Id.btnOnLights);

            //Text view
            txtLuzError = view.FindViewById<TextView>(Resource.Id.txtLuzError);

            //Set the web client
            client = new Client();

            //String state of lights
            stateLight1 = "false";
            stateLight2 = "false";
            stateLight3 = "false";
            stateLight4 = "false";
            stateLight5 = "false";

            //Functions for buttons
            btnOffLights.Click += TurnOffLights;
            btnOnLights.Click += TurnOnLights;

            //Update the lights state
            UpdateLights();

            //Funcion de la luz 1 
            btnLight1.CheckedChange += (s, e) => {
                if (e.IsChecked)
                    stateLight1 = "true";
                else
                    stateLight1 = "false";
                ChangeLightState();
            };

            //Funcion de la luz 2 
            btnLight2.CheckedChange += (s, e) => {
                if (e.IsChecked)
                    stateLight2 = "true";
                else
                    stateLight2 = "false";
                ChangeLightState();
            };

            //Funcion de la luz 3 
            btnLight3.CheckedChange += (s, e) => {
                if (e.IsChecked)
                    stateLight3 = "true";
                else
                    stateLight3 = "false";
                ChangeLightState();
            };

            //Funcion de la luz 4 
            btnLight4.CheckedChange += (s, e) => {
                if (e.IsChecked)
                    stateLight4 = "true";
                else
                    stateLight4 = "false";
                ChangeLightState();
            };

            //Funcion de la luz 5 
            btnLight5.CheckedChange += (s, e) => {
                if (e.IsChecked)
                    stateLight5 = "true";
                else
                    stateLight5 = "false";
                ChangeLightState();
            };

            return view;
        }

        private void UpdateLights()
        {
            try
            {
                string info = client.GetTextFromUrl();
                string[] words = info.Split('.');
                btnLight1.Checked = words[0].Equals("true");
                btnLight2.Checked = words[1].Equals("true");
                btnLight3.Checked = words[2].Equals("true");
                btnLight4.Checked = words[3].Equals("true");
                btnLight5.Checked = words[4].Equals("true");
                stateLight1 = words[0];
                stateLight2 = words[1];
                stateLight3 = words[2];
                stateLight4 = words[3];
                stateLight5 = words[4];
                if (info == null)
                    txtLuzError.Text = "No se puede conectar con el servidor";
                else
                    txtLuzError.Text = null;
            }
            catch (System.Exception)
            {
                txtLuzError.Text = "No se puede conectar con el servidor";
            }
        }

        private void ChangeLightState()
        {
            try
            {
                string infoLights = string.Format("{0}_{1}_{2}_{3}_{4}", stateLight1, stateLight2, stateLight3, stateLight4, stateLight5);
                client.SendLightByUrl(infoLights);
                Activity.RunOnUiThread((() => txtLuzError.Text = null));
            }
            catch (System.Exception)
            {
                Activity.RunOnUiThread((() => txtLuzError.Text = "No se puede conectar con el servidor"));
            }
        }

        private void TurnOffLights(object sender, EventArgs e)
        {
            try
            {
                btnLight1.Checked = false;
                btnLight2.Checked = false;
                btnLight3.Checked = false;
                btnLight4.Checked = false;
                btnLight5.Checked = false;
                stateLight1 = "false";
                stateLight2 = "false";
                stateLight3 = "false";
                stateLight4 = "false";
                stateLight5 = "false";
                ChangeLightState();
            }
            catch (Exception) { }
        }

        private void TurnOnLights(object sender, EventArgs e)
        {
            try
            {
                btnLight1.Checked = true;
                btnLight2.Checked = true;
                btnLight3.Checked = true;
                btnLight4.Checked = true;
                btnLight5.Checked = true;
                stateLight1 = "true";
                stateLight2 = "true";
                stateLight3 = "true";
                stateLight4 = "true";
                stateLight5 = "true";
                ChangeLightState();
            }
            catch (Exception) { }
        }
    }
}