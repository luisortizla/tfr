
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;

namespace TFEatery.Acitvities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class homePage : AppCompatActivity
    {
        //LLAVE PARA LLAMAR AL USUARIO INGRESADO EN EL LOGIN
        public static readonly string USER = "Usuario";
        TextView nameresttv;
        Button clientinfobtn, productosbtn, pedidosinfobtn;
        private MySqlConnection conn;
        
        public homePage()
        {
            MySqlConnectionStringBuilder con = new MySqlConnectionStringBuilder();
            con.Server = "mysql-10951-0.cloudclusters.net";
            con.Port = 10951;
            con.Database = "TapFood";
            con.UserID = "curecu";
            con.Password = "curecu123";

            conn = new MySqlConnection(con.ToString());

            conn.Open();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.homepage);
            nameresttv = FindViewById<TextView>(Resource.Id.nameresttv);
            clientinfobtn = FindViewById<Button>(Resource.Id.clientinfobtn);
            productosbtn = FindViewById<Button>(Resource.Id.productsbtn);
            pedidosinfobtn = FindViewById<Button>(Resource.Id.pedidosinfobtn);

            //CONVERSION DE LLAVE A STRING CON EL USUARIO INGRESADO EN EL LOGIN
            var iduser = Intent.GetStringExtra(USER);

            try
            {

                string sql = string.Format("SELECT * FROM TapFood.Restaurantes where (IdRestaurante = '{0}')", iduser.ToString());
                MySqlCommand cty = new MySqlCommand(sql, conn);
                MySqlDataReader plz;
                plz = cty.ExecuteReader();
                if (plz.Read())
                {
                    string Rest = (string)plz["NombreRestaurante"];
                    nameresttv.Text = Rest;
                    plz.Close();

                }
            }
            catch
            {
                Toast.MakeText(this, "me lleva la v...", ToastLength.Long).Show();
            }

             clientinfobtn.Click += Clientinfobtn_Click;
            productosbtn.Click += Productsbtn_Click;
            pedidosinfobtn.Click += Pedidosinfobtn_Click;
        }

        private void Pedidosinfobtn_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(pedidospage));
        }

        private void Productsbtn_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(productospage));
        }

        private void Clientinfobtn_Click(object sender, EventArgs e)
        {
            
            StartActivity(typeof(clientfeedback));
        }
    }
}
