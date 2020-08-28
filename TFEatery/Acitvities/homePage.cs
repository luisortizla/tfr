
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;

namespace TFEatery.Acitvities
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme.Splash", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class homePage : AppCompatActivity
    {
        //LLAVE PARA LLAMAR AL USUARIO INGRESADO EN EL LOGIN
        public static readonly string USER = "Usuario";
        TextView nameresttv;
        Button clientinfobtn, productosbtn, pedidosinfobtn, cuentas;
        private MySqlConnection conn;
        
        public homePage()
        {
            MySqlConnectionStringBuilder con = new MySqlConnectionStringBuilder();
            con.Server = "mysql-12128-0.cloudclusters.net";
            con.Port = 12160;
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
            cuentas = FindViewById<Button>(Resource.Id.cuentasinfobtn);

            //CONVERSION DE LLAVE A STRING CON EL USUARIO INGRESADO EN EL LOGIN
            var iduser = Intent.GetStringExtra(USER);

            try
            {

                string sql = string.Format("SELECT * FROM TapFood.Restaurantes where (IdRestaurante = '{0}')", iduser.ToString());
                Console.WriteLine(sql);
                MySqlCommand cty = new MySqlCommand(sql, conn);
                MySqlDataReader plz;
                plz = cty.ExecuteReader();
                if (plz.Read())
                {
                    string Rest = (string)plz["NombreRestaurante"];
                    nameresttv.Text = Rest;

                    string city = (string)plz["Ciudad"];
                    string plazaname = (string)plz["NombrePlaza"];
                    string tienda = "Tienda";
                    string restaurante = "Restaurante";
                    Console.WriteLine(plazaname);
                    if (plazaname != tienda && plazaname != restaurante)
                    {
                        string plaza = plazaname.Trim();
                        Console.WriteLine(plaza);
                        if (plz["LatitudRestaurante"] == DBNull.Value)
                        {
                            plz.Close();

                            string sql1 = string.Format("Select LatitudPlaza, LongitudPlaza from TapFood.Plaza where(NombrePlaza='{0}')", plaza);
                            Console.WriteLine(sql1);
                            MySqlCommand pla = new MySqlCommand(sql1, conn);
                            MySqlDataReader reader;
                            reader = pla.ExecuteReader();
                            reader.Read();
                            double lat = (double)reader["LatitudPlaza"];
                            double lng = (double)reader["LongitudPlaza"];
                            Console.WriteLine(lat+ "," + lng);
                            ISharedPreferences preff = PreferenceManager.GetDefaultSharedPreferences(this);
                            ISharedPreferencesEditor edit = preff.Edit();
                            edit.PutString("NombreRestaurante", Rest);
                            edit.PutString("Ciudad", city);
                            edit.PutString("NombrePlaza", plazaname);
                            edit.PutString("LatitudPlaza", lat.ToString());
                            edit.PutString("LongitudPlaza", lng.ToString());
                            edit.Apply();

                            plz.Close();
                        }
                    }

                    else
                    {
                        ISharedPreferences preff = PreferenceManager.GetDefaultSharedPreferences(this);
                        ISharedPreferencesEditor edit = preff.Edit();
                        edit.PutString("NombreRestaurante", Rest);
                        edit.PutString("Ciudad", city);
                        edit.PutString("NombrePlaza", plazaname);
                        edit.Apply();

                        plz.Close();
                    }
                }
            }
            catch
            {
                Toast.MakeText(this, "Refresca la pagina por favor", ToastLength.Long).Show();
            }

             clientinfobtn.Click += Clientinfobtn_Click;
            productosbtn.Click += Productsbtn_Click;
            pedidosinfobtn.Click += Pedidosinfobtn_Click;
            cuentas.Click += Cuentas_Click;
        }

        private void Cuentas_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(PedidosFinalizadosPage));
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
