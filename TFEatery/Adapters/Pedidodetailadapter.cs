using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using TFEatery.Entidades;

namespace TFEatery.Adapters
{
    public class Pedidodetailadapter:BaseAdapter<Pedidoadapdetail>
    {
        public List<Pedidoadapdetail> productos;
        Activity context;

        public Pedidodetailadapter(Activity context, List<Pedidoadapdetail> productos) : base()
        {
            this.context = context;
            this.productos = productos;
        }

        public override Pedidoadapdetail this[int position]
        {
            get { return productos[position]; }
        }

        public override int Count
        {
            get { return productos.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.pedidodetailproductoslayout, parent, false);

            var prod = productos[position];

            TextView producto = view.FindViewById<TextView>(Resource.Id.producto);
            producto.Text = prod.NombreProducto;


            TextView cantidad = view.FindViewById<TextView>(Resource.Id.cantidad);
            cantidad.Text = prod.Cantidad.ToString();

            return view;
        }
    }
}
