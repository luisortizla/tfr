
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using Bitmap = Android.Graphics.Bitmap;

namespace TFEatery.Acitvities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class clientfeedback : AppCompatActivity
    {

        private MySqlConnection conn;
        TextView namerestfbtv, habretv, hcierratv, descriptiontv;
        ImageView logoimage;
        Button updateclientfeedbackbtn;


        public clientfeedback()
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

            SetContentView(Resource.Layout.clientfeedbackpage);

            namerestfbtv = FindViewById<TextView>(Resource.Id.namerestfbtv);
            habretv = FindViewById<TextView>(Resource.Id.habretv);
            hcierratv = FindViewById<TextView>(Resource.Id.hcierratv);
            descriptiontv = FindViewById<TextView>(Resource.Id.descriptiontv);
            logoimage = FindViewById<ImageView>(Resource.Id.logoimage1);
            updateclientfeedbackbtn = FindViewById<Button>(Resource.Id.clientfeedbackupdatebtn);

            //SE UTILIZA EL USUARIO INGRESADO EN EL LOGIN QUE SE GUARDO CON SHARED PREFERENCES
            ISharedPreferences preff = PreferenceManager.GetDefaultSharedPreferences(this);
            var jee = preff.GetString("Usuario", "");
            

            string prev = string.Format("select DescripcionRestaurante from TapFood.Restaurantes where (IdRestaurante='{0}')", jee.ToString());
            MySqlCommand fff = new MySqlCommand(prev, conn);
            MySqlDataReader ccc;
            ccc = fff.ExecuteReader();
            ccc.Read();
            if (ccc.IsDBNull(0))
            {
                Toast.MakeText(this, "Por favor actualiza tus datos de vista al cliente", ToastLength.Long).Show();
                ccc.Close();

            }
            else
            {
                ccc.Close();
                string sql = string.Format("SELECT * FROM TapFood.Restaurantes where (IdRestaurante = '{0}')", jee);
                MySqlCommand cty = new MySqlCommand(sql, conn);
                MySqlDataReader plz;
                plz = cty.ExecuteReader();
                plz.Read();

                //SE LEEN LOS RESULTADO DEL QUERY SQL Y SE RELAIZA EL CAST PARA POSTERIORMENTE INIDCAR LA COLUMANDE LA TABLA MYSQL Y FINALMENTE EL VALOR OBTENIDO EN LA CONSULTA SE ASIGNA A UN ELEMENTO DEL LAYOOUT
                //EL METODO DE ABAJO SE USA PARA MOSTAR UNA IMAGEN DE MYSQL
                byte [] img = (byte[])plz["LogoRestaurante"];
                Bitmap mdg = BitmapFactory.DecodeByteArray(img, 0, img.Length);
                logoimage.SetImageBitmap(mdg);
                
                string Rest = (string)plz["NombreRestaurante"];
                namerestfbtv.Text = Rest;
                string abro = (string)plz["HoraApertura"];
                habretv.Text = abro.ToString();
                string cierro = (string)plz["HoraCierre"];
                hcierratv.Text = cierro.ToString();
                string descr = (string)plz["DescripcionRestaurante"];
                descriptiontv.Text = descr;
                
                plz.Close();
            }


            updateclientfeedbackbtn.Click += Updateclientfeedbackbtn_Click;


        }

        

        private void Updateclientfeedbackbtn_Click(object sender, EventArgs e)
        {
            conn.Close();
            StartActivity(typeof(clientfeedbackupdate));
        }
    }
}
