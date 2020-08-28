using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using TFEatery.Entidades;

namespace TFEatery.Adapters
{
    public class HistorialPedidosAdapter:BaseAdapter<HistorialPedidos>
    {
        public List<HistorialPedidos> pedidos;
        Activity context;

        public HistorialPedidosAdapter(Activity context, List<HistorialPedidos> pedidos) : base()
        {
            this.context = context;
            this.pedidos = pedidos;
        }

        public override HistorialPedidos this[int position]
        {
            get { return pedidos[position]; }
        }

        public override int Count
        {
            get { return pedidos.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.AdapHistorialPedidosLayout, parent, false);

            var prod = pedidos[position];

            TextView idp = view.FindViewById<TextView>(Resource.Id.idpedid);
            idp.Text = prod.IdPedido.ToString();

            TextView venta = view.FindViewById<TextView>(Resource.Id.venta);
            venta.Text = prod.Venta.ToString();

            return view;
        }
    }
}
