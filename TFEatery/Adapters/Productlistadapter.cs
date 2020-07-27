using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Preferences;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using TFEatery.Entidades;

namespace TFEatery.Adapters
{
    public class Productlistadapter:BaseAdapter<Producto>
    {
        public List<Producto> productos;
        Activity context;
        public MySqlConnection conn;
        

        public Productlistadapter(Activity context, List<Producto> productos) : base()
        {
            this.context = context;
            this.productos = productos;

        }

        //PRIMER METOD.O
        public override int Count
        {
            get { return productos.Count; }
        }

        //SEGUNDO METOD.O
        public override Producto this[int position]
        {
            get { return productos[position]; }
        }

        //TERCER METOD.O
        public override long GetItemId(int position)
        {
            return position;
        }

        //CUARTO METOD.O
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.listproductosadapterlayout, parent, false);
            
            var prod = productos[position];

            ImageView prodimage = view.FindViewById<ImageView>(Resource.Id.productoimageiv);
            //var imagebit = GetBitmap(prod.FotoProducto);
            //prodimage.SetImageBitmap(imagebit);
            byte[] veremos = prod.FotoProducto;
            Bitmap mdg = BitmapFactory.DecodeByteArray(veremos, 0, veremos.Length);
            prodimage.SetImageBitmap(mdg);

            TextView prodnombre = view.FindViewById<TextView>(Resource.Id.productonametv);
            prodnombre.Text = prod.NombreProducto;

            TextView proddescripcion = view.FindViewById<TextView>(Resource.Id.productdescriptiontv);
            proddescripcion.Text = prod.Descripcion;

            TextView proddeadline = view.FindViewById<TextView>(Resource.Id.deadlinetv);
            proddeadline.Text = prod.TiempoEntrega.ToString() + " Min.";

            TextView proddescuento = view.FindViewById<TextView>(Resource.Id.descuentotv);
            proddescuento.Text = "Descuento " + prod.Descuento.ToString() + "%";

            TextView prodprecio = view.FindViewById<TextView>(Resource.Id.pricetv);
            prodprecio.Text = "$" + prod.PrecioProducto.ToString();

            return view;
        }

        private Bitmap GetBitmap (byte[] img)
        {
            Bitmap mdg = BitmapFactory.DecodeByteArray(img, 0, img.Length);
            return mdg;
        }

    }
}
