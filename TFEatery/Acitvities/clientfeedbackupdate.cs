
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

using System.Text;

using Android.App;
using Android.Content;
using Android.Database;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media;
using Android.OS;
using Android.Preferences;
using Android.Provider;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Nio;
using MySql.Data.MySqlClient;
using Xamarin.Forms.Platform.Android;
using static Android.Graphics.Bitmap;
using Bitmap = Android.Graphics.Bitmap;
using File = System.IO.File;
using Stream = System.IO.Stream;
using Uri = Android.Net.Uri;

namespace TFEatery.Acitvities
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme.Splash", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class clientfeedbackupdate : AppCompatActivity
    {
        public static readonly string USER = "Usuario";
        EditText restdescriptiontxt;
        Button regfinishbtn, insertlogobtn;
        Spinner abrespn, cierrespn;
        ImageView logoimage;
        private MySqlConnection conn;
        string apertura, clausura, descripcion;
        bool resultado;
        


        public static readonly int PickImageId = 1000;

        public clientfeedbackupdate()
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

            SetContentView(Resource.Layout.clientfeedbackupdatelayout);

            restdescriptiontxt = FindViewById<EditText>(Resource.Id.restdescriptiontxt);
            regfinishbtn = FindViewById<Button>(Resource.Id.regfinishbtn);
            insertlogobtn = FindViewById<Button>(Resource.Id.insertlogobtn);
            abrespn = FindViewById<Spinner>(Resource.Id.abrespn);
            cierrespn = FindViewById<Spinner>(Resource.Id.cierrespn);
            logoimage = FindViewById<ImageView>(Resource.Id.logoimage);
            

            
            //SE INVOCA EL DATO GUARDO CON SHARED PREFERENCES EN EL LOGIN
            ISharedPreferences preff = PreferenceManager.GetDefaultSharedPreferences(this);
            var jee = preff.GetString("Usuario", "");

            var horas = ArrayAdapter<string>.CreateFromResource(this, Resource.Array.horas, Android.Resource.Layout.SimpleSpinnerItem);
            horas.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            abrespn.Adapter = horas;
            cierrespn.Adapter = horas;

            var iduser = Intent.GetStringExtra(USER);

            insertlogobtn.Click += Insertlogo_Click;
            regfinishbtn.Click += Regfinishbtn_Click;
            
        }

        //EL INTENTO DE ACCEDER A LA CAMARA Y ELEGIR LA IMAGEN
        private void  Insertlogo_Click(object sender, EventArgs e)
        {
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
            
        }


        //LA ACTIVIDAD RESULTADO DEL INTENTO DE ARRIBA
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
            {
                ISharedPreferences preff = PreferenceManager.GetDefaultSharedPreferences(this);
                var jee = preff.GetString("Usuario", "");

                //SE AGREGA LA IMAGEN AL IMAGEVIEW Y SU URL INTERNO
                string path;
                Uri uri = data.Data;
                path = GetPathToImage(uri);
                Bitmap cool = BitmapFactory.DecodeFile(path);
                logoimage.SetImageBitmap(cool);

                //MEDIANTE EL URL INTERNO SE CREA UN BITMAP INTERNO PARA POSTERIORMENTE SER CONVERTIDO EN BYTE[] Y ALMACENADO EN MYSQL
                Bitmap seco = Bitmap.CreateBitmap(BitmapFactory.DecodeFile(path));
                var nel = new MemoryStream();
                seco.Compress(Bitmap.CompressFormat.Png, 0, nel);
                byte [] bitmapData = nel.ToArray();

                //FORMA CORRECTA PARA SUBIR UNA IMAGEN A MYSQL
                var final = new MySqlCommand("UPDATE `TapFood`.`Restaurantes` SET  `LogoRestaurante`= @logo  WHERE(`IdRestaurante` = @iduser)",conn);
                final.Parameters.Add(new MySqlParameter("logo", MySqlDbType.MediumBlob)).Value = bitmapData;
                final.Parameters.Add(new MySqlParameter("iduser", MySqlDbType.String)).Value = jee.ToString();
                final.ExecuteNonQuery();
                Toast.MakeText(this, "Listo, hemos actualizado la imagen".ToString(), ToastLength.Long).Show();


            }
            
        }

        private void Regfinishbtn_Click(object sender, EventArgs e)
        {
            ISharedPreferences preff = PreferenceManager.GetDefaultSharedPreferences(this);
            var jee = preff.GetString("Usuario", "");

            apertura = abrespn.SelectedItem.ToString();
            clausura = cierrespn.SelectedItem.ToString();   
            descripcion = restdescriptiontxt.ToString();

         
            try
            {
                string sql = string.Format("UPDATE `TapFood`.`Restaurantes` SET `HoraApertura` = '{0}', `HoraCierre` = '{1}', `DescripcionRestaurante`='{2}' WHERE (`IdRestaurante` = '{3}')", apertura, clausura, restdescriptiontxt.Text, jee.ToString());
                MySqlCommand finishregister = new MySqlCommand(sql, conn);
                finishregister.ExecuteNonQuery();
                resultado = true;
            }
            catch
            {
                Toast.MakeText(this, "Por favor intentalo de nuevo en 30 segundos, gracias", ToastLength.Long).Show();
                resultado = false;
            }
            if (resultado == true)
            {
                Toast.MakeText(this, "Listo, hemos actualizado tu informacion!", ToastLength.Long).Show();
                Finish();
            }
            
        }

        
        //SE OBITENE EL URL INTERNO COMPLETO PARA PODER REALIZAR LA CONVERSION A BTYE[] Y ALMACENAMIENTO EN MYSQL
        private string GetPathToImage(Android.Net.Uri uri)
        {
            ICursor cursor = this.ContentResolver.Query(uri, null, null, null, null);
            cursor.MoveToFirst();
            string document_id = cursor.GetString(0);
            if (document_id.Contains(":"))
                document_id = document_id.Split(':')[1];
            cursor.Close();

            cursor = ContentResolver.Query(
            Android.Provider.MediaStore.Images.Media.ExternalContentUri,
            null, MediaStore.Images.Media.InterfaceConsts.Id + " = ? ", new String[] { document_id }, null);
            cursor.MoveToFirst();
            string path = cursor.GetString(cursor.GetColumnIndex(MediaStore.Images.Media.InterfaceConsts.Data));
            cursor.Close();

            return path;
        }


    }
}
