
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Database;
using Android.Graphics;
using Android.OS;
using Android.Preferences;
using Android.Provider;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using Uri = Android.Net.Uri;

namespace TFEatery.Acitvities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class agregarproducto : AppCompatActivity
    {
        public static readonly int PickImageId = 1000;
        private MySqlConnection conn;
        EditText nameproductocreadotxt, productodescpcioncreaciontxt, precioproductotxt, timpoentregatxt, descuentocreatetxt;
        ImageView productoimagencreacion;
        Button creacionproductofinalizarbtn, fotoproductobtn;
        Spinner productotipodecomidaspn;

        
        public agregarproducto()
        {
            MySqlConnectionStringBuilder con = new MySqlConnectionStringBuilder();
            con.Server = "mysql-10951-0.cloudclusters.net";
            con.Port = 10951;
            con.Database = "TapFood";
            con.UserID = "curecu";
            con.Password = "curecu123";

            conn = new MySqlConnection(con.ToString());

            conn.Open();

            Random rnd = new Random();
            double r = rnd.Next(10000, 99999);
            string text = new string("TFFP");
            string IdRestaurante = text + r;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.agregarproductolayout);
            nameproductocreadotxt = FindViewById<EditText>(Resource.Id.nameproductocreadotxt);
            productodescpcioncreaciontxt = FindViewById<EditText>(Resource.Id.productodescpcioncreaciontxt);
            precioproductotxt = FindViewById<EditText>(Resource.Id.precioproductotxt);
            timpoentregatxt = FindViewById<EditText>(Resource.Id.timpoentregatxt);
            descuentocreatetxt = FindViewById<EditText>(Resource.Id.descuentocreatetxt);
            productoimagencreacion = FindViewById<ImageView>(Resource.Id.productoimagencreacion);
            creacionproductofinalizarbtn = FindViewById<Button>(Resource.Id.creacionproductofinalizarbtn);
            productotipodecomidaspn = FindViewById<Spinner>(Resource.Id.productotipodecomidaspn);
            fotoproductobtn = FindViewById<Button>(Resource.Id.fotoproductobtn);

            string sql = string.Format("SELECT TipoDeComida FROM TapFood.Comida");
            MySqlCommand cty = new MySqlCommand(sql, conn);
            MySqlDataReader plz;
            plz = cty.ExecuteReader();

            var tipos = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem);
            while (plz.Read())
            {
                tipos.Add(" " + plz["TipoDeComida"].ToString() + "");
            }
            productotipodecomidaspn.Adapter = tipos;
            plz.Close();

            fotoproductobtn.Click += fotoproductobtn_Click;
            creacionproductofinalizarbtn.Click += Creacionproductofinalizarbtn_Click;

            
        }


        private void fotoproductobtn_Click(object sender, EventArgs e)
        {
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
            {
                
                //SE AGREGA LA IMAGEN AL IMAGEVIEW Y SU URL INTERNO
                string path;
                Uri uri = data.Data;
                path = GetPathToImage(uri);
                Bitmap cool = BitmapFactory.DecodeFile(path);
                productoimagencreacion.SetImageBitmap(cool);

                //MEDIANTE EL URL INTERNO SE CREA UN BITMAP INTERNO PARA POSTERIORMENTE SER CONVERTIDO EN BYTE[] Y ALMACENADO EN MYSQL
                /*Bitmap seco = Bitmap.CreateBitmap(BitmapFactory.DecodeFile(path));
                var nel = new MemoryStream();
                seco.Compress(Bitmap.CompressFormat.Png, 0, nel);
                byte[] bitmapData = nel.ToArray();*/

                //ABAJO SE MUESTRA LA FORMA DE GUARDAR EL URL DE LA IMAGEN PARA SER USADA POSTERIORMENTE EN LA CRECACION DEL PRODUCTO EN MYSQL
                ISharedPreferences coladeraton = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor edit = coladeraton.Edit();
                edit.PutString("Path",path);
                edit.Apply();

            }

        }

        private void Creacionproductofinalizarbtn_Click(object sender, EventArgs e)
        {
            ISharedPreferences preff = PreferenceManager.GetDefaultSharedPreferences(this);
            var jee = preff.GetString("Usuario", "");

            ISharedPreferences coladeraton = PreferenceManager.GetDefaultSharedPreferences(this);
            var path = coladeraton.GetString("Path", "");

            string tipocomida = productotipodecomidaspn.SelectedItem.ToString();

            Random rnd = new Random();
            double r = rnd.Next(10000, 99999);
            string text = new string("TFFP");
            string IdRestaurante = text + r;

            Bitmap seco = Bitmap.CreateBitmap(BitmapFactory.DecodeFile(path));
            var nel = new MemoryStream();
            seco.Compress(Bitmap.CompressFormat.Png, 0, nel);
            byte[] bitmapData = nel.ToArray();

            try
            {
                string rest1 = new string(IdRestaurante);
                var sql = new MySqlCommand("INSERT INTO `TapFood`.`Producto` (`IdProducto`, `NombreProducto`, `IdRestaurante`, `NombreRestaurante`, `TipoDeComida`, `Descripcion`, `PrecioProducto`, `TiempoEntrega`, `Descuento`, `FotoProducto`) VALUES (@idprod, @nombreprod, @idrest, @nombrerest, @tipocomida, @descripcionprod, @precioprod, @tiempoentrega, @descuentoprod, @fotoprod)", conn);
                sql.Parameters.Add(new MySqlParameter("idprod", MySqlDbType.String)).Value = rest1;
                sql.Parameters.Add(new MySqlParameter("nombreprod", MySqlDbType.String)).Value = nameproductocreadotxt.Text;
                sql.Parameters.Add(new MySqlParameter("idrest", MySqlDbType.String)).Value = jee.ToString();
                sql.Parameters.Add(new MySqlParameter("nombrerest", MySqlDbType.String)).Value = GetNameRest(jee.ToString());
                sql.Parameters.Add(new MySqlParameter("tipocomida", MySqlDbType.String)).Value = tipocomida;
                sql.Parameters.Add(new MySqlParameter("descripcionprod", MySqlDbType.String)).Value = productodescpcioncreaciontxt.Text;
                sql.Parameters.Add(new MySqlParameter("precioprod", MySqlDbType.Float)).Value = precioproductotxt.Text;
                sql.Parameters.Add(new MySqlParameter("tiempoentrega", MySqlDbType.Int32)).Value = timpoentregatxt.Text;
                sql.Parameters.Add(new MySqlParameter("descuentoprod", MySqlDbType.Int32)).Value = descuentocreatetxt.Text;
                sql.Parameters.Add(new MySqlParameter("fotoprod", MySqlDbType.MediumBlob)).Value = bitmapData;
                sql.ExecuteNonQuery();
                Toast.MakeText(this, "Listo, has creado tu nuevo producto!".ToString(), ToastLength.Long).Show();

            }
            catch
            {
                Toast.MakeText(this, "Por favor verifica tus datos y vuelve a intentarlo.", ToastLength.Long).Show();
            }
        }

        private string GetNameRest(string idrest)
        {
            
            string sql = string.Format("SELECT * FROM TapFood.Restaurantes where (IdRestaurante = '{0}')", idrest);
            MySqlCommand cty = new MySqlCommand(sql, conn);
            MySqlDataReader plz;
            plz = cty.ExecuteReader();
            plz.Read();
            string Rest = (string)plz["NombreRestaurante"];
            plz.Close();

            return Rest;
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
