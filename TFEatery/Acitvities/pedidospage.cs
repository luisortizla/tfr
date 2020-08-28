
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using TFEatery.Adapters;
using TFEatery.Entidades;
using AlertDialog = Android.App.AlertDialog;

namespace TFEatery.Acitvities
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme.Splash", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class pedidospage : AppCompatActivity
    {
        private SwipeRefreshLayout refreshLayout;
        public List<Pedidosadap> pedidos = new List<Pedidosadap>();
        ListView list;
        private MySqlConnection conn;

        public pedidospage()
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
            SetContentView(Resource.Layout.pedidoslayout);
            list = FindViewById<ListView>(Resource.Id.pedidoslist);
            refreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayout);

            ISharedPreferences preff = PreferenceManager.GetDefaultSharedPreferences(this);
            var jee = preff.GetString("Usuario", "");

            string prev = string.Format("Select IdPedido, GROUP_CONCAT(Confirmada Order by Confirmada desc SEPARATOR ' ') as Resolucion from TapFood.Pedido where (Recolectada ='NO' and IdRestaurante = '{0}') group by IdPedido;", jee.ToString());
            //string prev = string.Format("select * from TapFood.Pedido where IdRestaurante='{0}'", jee.ToString());
            MySqlCommand fff = new MySqlCommand(prev, conn);
            MySqlDataReader ccc;
            ccc = fff.ExecuteReader();

            while (ccc.Read())
            {
                Pedidosadap pedido = new Pedidosadap();
                /*pedido.irrelevante = Convert.ToInt32(ccc["irrelevante"].ToString());
                pedido.IdPedido = ccc["IdPedido"].ToString();
                pedido.IdUsuario = ccc["IdUsuario"].ToString();
                pedido.NombreUsuario = ccc["NombreUsuario"].ToString();
                pedido.IdRestaurante = ccc["IdRestaurante"].ToString();
                pedido.IdProducto = ccc["IdProducto"].ToString();
                pedido.NombreProducto = ccc["NombreProducto"].ToString();
                pedido.PrecioProducto = (float)Convert.ToDouble(ccc["PrecioProducto"].ToString());
                pedido.Cantidad = Convert.ToInt32(ccc["Cantidad"].ToString());
                pedido.IdRepartidor = ccc["IdRepartidor"].ToString();
                pedido.NombreRepartidor = ccc["NombreRepartidor"].ToString();
                pedido.TipoDePago = ccc["TipoDePago"].ToString();
                pedido.LongitudPlaza = Convert.ToDouble(ccc["LongitudPlaza"].ToString());
                pedido.LatitudPlaza = Convert.ToDouble(ccc["LatitudPlaza"].ToString());
                pedido.LongitudUsuario = Convert.ToDouble(ccc["LongitudUsuario"].ToString());
                pedido.LatitudUsuario = Convert.ToDouble(ccc["LatitudUsuario"].ToString());
                pedido.Creada = ccc["Creada"].ToString();
                pedido.Confirmada = ccc["Confirmada"].ToString();
                pedido.Recolectada = ccc["Recolectada"].ToString();
                pedido.Entregada = ccc["Entregada"].ToString();*/
                pedido.IdPedido = ccc["IdPedido"].ToString();
                pedido.Resolucion = ccc["Resolucion"].ToString();

                pedidos.Add(pedido);
            }

            ccc.Close();
            list.Adapter = new pedidosgridadapter(this, pedidos);
            list.ChoiceMode = ChoiceMode.Single;
            //list.ItemSelected += List_ItemSelected;
            list.ItemClick += List_ItemClick;

            refreshLayout.SetColorSchemeColors(Color.Red, Color.Green, Color.Blue, Color.Yellow);
            refreshLayout.Refresh += RefreshLayout_Refresh;

        }

        private void RefreshLayout_Refresh(object sender, EventArgs e)
        {
            BackgroundWorker work = new BackgroundWorker();
            work.DoWork += Work_DoWork;
            work.RunWorkerCompleted += Work_RunWorkerCompleted;
            work.RunWorkerAsync();
        }
        private void Work_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            refreshLayout.Refreshing = false;
        }
        private void Work_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(1000);
        }

        private void List_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ISharedPreferences preff = PreferenceManager.GetDefaultSharedPreferences(this);
            var jee = preff.GetString("Usuario", "");

            string idorder = pedidos[e.Position].IdPedido.ToString();
            string pala = pedidos[e.Position].Resolucion.ToString();
            Console.WriteLine(pala);
            int count = pala.Split(' ').Length;
            if(count == 1)
            {
                string election = pala;
                Console.WriteLine(election);
                Intent info = new Intent(this, typeof(pedidodetailpage));
                info.PutExtra(pedidodetailpage.IDORDER, idorder);
                info.PutExtra(pedidodetailpage.ESTATUS, election);
                StartActivity(info);
            }
            else
            {
                var palaarray = pala.Split(' ');
                string election = palaarray[palaarray.Length -1];
                Console.WriteLine(election);
                Intent info = new Intent(this, typeof(pedidodetailpage));
                info.PutExtra(pedidodetailpage.IDORDER, idorder);
                info.PutExtra(pedidodetailpage.ESTATUS, election);
                StartActivity(info);
            }

        }

        //            string lelo = string.Format("UPDATE TapFood.Pedido SET Confirmada ='Confirmada' where (IdPedido = '{0}' and IdRestaurante ='{1}')", idorder, jee.ToString());

    }
}

