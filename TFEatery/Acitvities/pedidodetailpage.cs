
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
using TFEatery.Adapters;
using TFEatery.Entidades;

namespace TFEatery.Acitvities
{
    [Activity(Label = "pedidodetailpage")]
    public class pedidodetailpage : AppCompatActivity
    {
        internal static readonly string IDORDER = "IdPedido";
        internal static readonly string ESTATUS = "Estatus";
        public List<Pedidoadapdetail> productos = new List<Pedidoadapdetail>();
        ListView listpedidosdetail;
        private MySqlConnection conn;
        TextView pedidotv, clientnametv;

        public pedidodetailpage()
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
            SetContentView(Resource.Layout.pedidodetaillayout);
            listpedidosdetail = FindViewById<ListView>(Resource.Id.listpedidodetail);
            pedidotv = FindViewById<TextView>(Resource.Id.pedidotv);
            clientnametv = FindViewById<TextView>(Resource.Id.clientnametv);

            ISharedPreferences preff = PreferenceManager.GetDefaultSharedPreferences(this);
            var jee = preff.GetString("Usuario", "");
            var idpedido = Intent.GetStringExtra(IDORDER);
            var estatuss = Intent.GetStringExtra(ESTATUS);

            string sql = string.Format("SELECT NombreUsuario FROM TapFood.Pedido where (IdPedido = '{0}')", idpedido.ToString());
            MySqlCommand cty = new MySqlCommand(sql, conn);
            MySqlDataReader plz;
            plz = cty.ExecuteReader();
            plz.Read();
            string nombre = plz[0].ToString();
            plz.Close();

            clientnametv.Text = nombre;
            pedidotv.Text = idpedido.ToString();

            string sql2 = string.Format("Select NombreProducto, Cantidad from TapFood.Pedido where (IdPedido = '{0}' AND IdRestaurante = '{1}')", idpedido.ToString(), jee.ToString());
            Console.WriteLine(sql2);
            MySqlCommand cty2 = new MySqlCommand(sql2, conn);
            MySqlDataReader plz2;
            plz2 = cty2.ExecuteReader();
            while (plz2.Read())
            {
                Pedidoadapdetail producto = new Pedidoadapdetail();
                producto.NombreProducto = plz2["NombreProducto"].ToString();
                producto.Cantidad = Convert.ToInt32(plz2["Cantidad"].ToString());
                productos.Add(producto);
            }
            plz2.Close();
            listpedidosdetail.Adapter = new Pedidodetailadapter(this, productos);

            Console.WriteLine(estatuss);
            if (estatuss.ToString() == "NO")
            {
                string sql3 = string.Format("Select irrelevante from TapFood.Pedido where (IdPedido = '{0}' AND IdRestaurante = '{1}' and Confirmada ='NO')   ", idpedido.ToString(), jee.ToString());
                Console.WriteLine(sql3);
                Console.WriteLine(sql3);
                MySqlCommand cty3 = new MySqlCommand(sql3, conn);
                MySqlDataReader plz3;
                plz3 = cty3.ExecuteReader();
                plz3.Read();
                if (plz3.HasRows)
                {
                    plz3.Close();
                    string sql4 = string.Format("UPDATE `TapFood`.`Pedido` SET `Confirmada` = 'Confirmada' where (IdPedido = '{0}' AND IdRestaurante = '{1}' and Confirmada='NO')", idpedido.ToString(), jee.ToString());
                    Console.WriteLine(sql4);
                    MySqlCommand cmd = new MySqlCommand(sql4, conn);
                    cmd.ExecuteNonQuery();
                }

            }
        }
    }

}

