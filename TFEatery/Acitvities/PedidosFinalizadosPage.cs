
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

namespace TFEatery.Acitvities
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme.Splash", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class PedidosFinalizadosPage : AppCompatActivity
    {
        List<HistorialPedidos> historial = new List<HistorialPedidos>();
        MySqlConnection conn;
        ListView Historiallist;
        private SwipeRefreshLayout refreshLayout3;

        public PedidosFinalizadosPage()
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
            SetContentView(Resource.Layout.PedidosFinalizadosLayout);
            Historiallist = FindViewById<ListView>(Resource.Id.historialpedidoslist);
            refreshLayout3 = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayout3);
            // Create your application here
            ISharedPreferences preff = PreferenceManager.GetDefaultSharedPreferences(this);
            var jee = preff.GetString("Usuario", "");
            
            string hist = string.Format("Select IdPedido, SUM(PrecioProducto) as Venta From TapFood.Pedido where(IdRestaurante= '{0}') group by IdPedido", jee.ToString());
            MySqlCommand cmd = new MySqlCommand(hist, conn);
            MySqlDataReader read;
            read = cmd.ExecuteReader();
            while (read.Read())
            {
                HistorialPedidos historia = new HistorialPedidos();
                historia.IdPedido = read["IdPedido"].ToString();
                historia.Venta =(float)read["Venta"];
                historial.Add(historia);
            }
            read.Close();
            Historiallist.Adapter = new HistorialPedidosAdapter(this,historial);
            Historiallist.ChoiceMode = ChoiceMode.Single;

            Historiallist.ItemClick += Productslislv_ItemClick;
            refreshLayout3.SetColorSchemeColors(Color.Red, Color.Green, Color.Blue, Color.Yellow);
            refreshLayout3.Refresh += RefreshLayout_Refresh;
        }

        private void Productslislv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(pedidodetailpage));
            intent.PutExtra(pedidodetailpage.IDPED,historial[e.Position].IdPedido.ToString());
            StartActivity(intent);
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
            refreshLayout3.Refreshing = false;
        }
        private void Work_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(1000);
        }
    }
}
