using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Xamarin.Android;

using Xamarin.Essentials;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using TFEatery.MySQLconector;
using AlertDialog = Android.App.AlertDialog;

namespace TFEatery.Acitvities
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme.Splash", MainLauncher = false, NoHistory = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class registerpage : AppCompatActivity
    {

        EditText restnametxt, responsabletxt, emailresttxt, passrgtxt, phoneresttxt, bancounttxt;
        Spinner cityspn, plazaspn, bancospn;
        Button nextbtn;
        private MySqlConnection conn;
        string ciudad, plaza, banco, rest1;
        bool resultado;

        public registerpage()
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



            SetContentView(Resource.Layout.registerpage);

            restnametxt = FindViewById<EditText>(Resource.Id.restnametxt);
            responsabletxt = FindViewById<EditText>(Resource.Id.responsabletxt);
            emailresttxt = FindViewById<EditText>(Resource.Id.emailresttxt);
            passrgtxt = FindViewById<EditText>(Resource.Id.passrgtxt);
            phoneresttxt = FindViewById<EditText>(Resource.Id.phoneresttxt);
            bancounttxt = FindViewById<EditText>(Resource.Id.bancountktxt);
            nextbtn = FindViewById<Button>(Resource.Id.nextbtn);
            cityspn = FindViewById<Spinner>(Resource.Id.cityspn);
            plazaspn = FindViewById<Spinner>(Resource.Id.plazaspn);
            bancospn = FindViewById<Spinner>(Resource.Id.bancospn);

            nextbtn.Click += Nextbtn_Click;



            var spinnerbanco = ArrayAdapter<string>.CreateFromResource(this, Resource.Array.Banco, Android.Resource.Layout.SimpleSpinnerItem);
            spinnerbanco.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            bancospn.Adapter = spinnerbanco;



            string sql = string.Format("SELECT NombrePlaza, Ciudad FROM TapFood.Plaza");
            MySqlCommand cty = new MySqlCommand(sql, conn);
            MySqlDataReader plz;
            plz = cty.ExecuteReader();


            var plazas = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem);
            var cities = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem);

            //MIENTRAS EL QUERY EJECUTADO SIGA ENCONTRANDO RESULTADOS, SE IRAN AGREGANDO AL ARRAY PARA OFRECER SOLUCIONES EN EL SPINNER DE CIUDAD Y NOMBRE DE PLAZA.
            while (plz.Read())
            {
                cities.Add(" " + plz["Ciudad"].ToString() + "");
                plazas.Add(" " + plz["NombrePlaza"].ToString() + " ");
            }
            cityspn.Adapter = cities;

            plazaspn.Adapter = plazas;
            plz.Close();
        }

        private void Nextbtn_Click(object sender, EventArgs e)
        {

            ciudad = cityspn.SelectedItem.ToString();
            plaza = plazaspn.SelectedItem.ToString();
            banco = bancospn.SelectedItem.ToString();

            //SE GENERA EL ID MEDIANTA UN CODIGO Y 5 NUMEROS ALEATORIOS
            Random rnd = new Random();
            double r = rnd.Next(10000, 99999);
            string text = new string("TFCW");
            string IdRestaurante = text + r;

            try
            {
                rest1 = new string(IdRestaurante);
                string sql = string.Format("INSERT INTO `TapFood`.`Restaurantes` (`IdRestaurante`, `NombreRestaurante`, `Ciudad`, `NombrePlaza`, `ResponsableRestaurante`, `EmailRestaurante`, `ContraseñaRestaurante`, `TelefonoRestaurante`, `CuentaDepositoRestaurante`, `Banco`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')", rest1, restnametxt.Text, ciudad, plaza, responsabletxt.Text, emailresttxt.Text, passrgtxt.Text, phoneresttxt.Text, bancounttxt.Text, banco);
                MySqlCommand logincmdregister = new MySqlCommand(sql, conn);
                logincmdregister.ExecuteNonQuery();
                resultado = true;

            }
            catch
            {
                Toast.MakeText(this, "Por favor verifica tus datos y vuelve a intentarlo.", ToastLength.Long).Show();
                resultado = false;
            }
            if (resultado == true)
            {

                //CUADRO DE DIALOGO CON EL USUARIO CREADO Y LA CONTRASEñA INGRESADA
                AlertDialog.Builder informacion = new AlertDialog.Builder(this);
                AlertDialog create = informacion.Create();
                create.SetTitle("Datos de acceso:");
                create.SetMessage("Usuario: " + rest1 +
                    "\nContraseña: " + passrgtxt.Text);
                create.SetButton("Ok", (create, EventArgs) =>
                {
                    StartActivity(typeof(MainActivity));
                });
                create.Show();

               
            }
        }
    }
}
