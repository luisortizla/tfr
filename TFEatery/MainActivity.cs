using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using TFEatery.Entidades;
using TFEatery.MySQLconector;
using System;
using TFEatery.Acitvities;
using Android.Content;
using Android.Preferences;

namespace TFEatery
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        EditText userlogintxt, passlogintxt;
        Button loginbtn, registerbtn;
        private MySqlConnection conn;


        public MainActivity()
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




            SetContentView(Resource.Layout.loginpage);

            userlogintxt = FindViewById<EditText>(Resource.Id.userlogintxt);
            passlogintxt = FindViewById<EditText>(Resource.Id.passlogintxt);
            loginbtn = FindViewById<Button>(Resource.Id.loginbtn);
            registerbtn = FindViewById<Button>(Resource.Id.registerbtn);

            registerbtn.Click += Registerbtn_Click;
            loginbtn.Click += Loginbtn_Click;

        }

        private void Loginbtn_Click(object sender, EventArgs e)
        {
            try
            {
                
                string sql1 = string.Format("Select * From TapFood.Restaurantes Where (IdRestaurante, ContraseñaRestaurante) =('{0}','{1}')", userlogintxt.Text, passlogintxt.Text);
                MySqlCommand loginverid = new MySqlCommand(sql1, conn);
                MySqlDataReader usr;
                usr = loginverid.ExecuteReader();
                if (usr.HasRows)
                {
                    //PASAR EL USUARIO AL HOME PAGE
                    Intent pis = new Intent(this, typeof(homePage));
                    pis.PutExtra(homePage.USER, userlogintxt.Text);

                    string jee = userlogintxt.ToString();

                    //GUARDAR EL USUARIO DE INICIO DE SESION PARA QUE SEA USADO EN CUALQUIER OTRA ACTIVIDAD
                    ISharedPreferences preff = PreferenceManager.GetDefaultSharedPreferences(this);
                    ISharedPreferencesEditor edit = preff.Edit();
                    edit.PutString("Usuario", userlogintxt.Text.ToString());
                    
                    edit.Apply();

                    StartActivity(pis);
                    
                    Toast.MakeText(this, "Has ingresado!", ToastLength.Long).Show();
                 
                }
                else
                {
                    Toast.MakeText(this, "Tus datos son errones, revisalor por favor.", ToastLength.Long).Show();

                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show(); ;
            }
        }

        private void Registerbtn_Click(object sender, EventArgs e)
        {
           
            StartActivity(typeof(registerpage));
        }

        
    }
}