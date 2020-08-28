
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
using Com.Mapbox.Mapboxsdk;
using Com.Mapbox.Mapboxsdk.Camera;
using Com.Mapbox.Mapboxsdk.Geometry;
using Com.Mapbox.Mapboxsdk.Maps;
using MySql.Data.MySqlClient;

namespace TFEatery.Acitvities
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme.Splash", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MapPage : AppCompatActivity, IOnMapReadyCallback
    {
        MapView mapView = null;
        MapboxMap mapbox = null;
        Button selectlocationbtn;
        MySqlConnection conn;

        public MapPage()
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
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            string x = "sk.eyJ1IjoibHVpc29ydGl6cyIsImEiOiJja2RkbWJ5cWExNnhoMnlwY3hwZXV6ZTE1In0.zdLAf030Z_wllV9R0WSwbw";
            Mapbox.GetInstance(this, x);
            SetContentView(Resource.Layout.MapLayout);
            selectlocationbtn = FindViewById<Button>(Resource.Id.estaubicacion);
            mapView = FindViewById<MapView>(Resource.Id.mapView);
            //mapView = new MapView(this);
            mapView.OnCreate(savedInstanceState);
            mapView.GetMapAsync(this);
            // Create your application here
        }

        public void OnMapReady(MapboxMap mapbox)
        {
            this.mapbox = mapbox;
            mapbox.SetStyle("mapbox://styles/luisortizs/ckdeotksa59vs1imw35jiqemz");
            double ltd = 14.910448;
            double lng = -92.264833;

            var position = new CameraPosition.Builder()
                           .Target(new LatLng(ltd, lng))
                           .Zoom(13)
                           .Build();

            mapbox.AnimateCamera(CameraUpdateFactory.NewCameraPosition(position));
        }

        protected override void OnStart()
        {
            base.OnStart();
            mapView?.OnStart();
        }

        protected override void OnResume()
        {
            base.OnResume();
            mapView?.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
            mapView?.OnPause();
        }

        protected override void OnStop()
        {
            base.OnStop();
            mapView?.OnStop();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            mapView?.OnSaveInstanceState(outState);
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();
            mapView?.OnLowMemory();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            mapView?.OnDestroy();
        }

        private void Selectlocationbtn_Click(object sender, EventArgs e)
        {
            var positioncam = mapbox.CameraPosition;
            LatLng maplocation = positioncam.Target;
            double ltdd = mapbox.CameraPosition.Target.Latitude;
            double lgnn = mapbox.CameraPosition.Target.Longitude;

            ISharedPreferences preff = PreferenceManager.GetDefaultSharedPreferences(this);
            var jee = preff.GetString("Usuario", "");

            string sql = string.Format("Update TapFood.Restaurantes SET LatitudRestaurante='{0}', LongitudRestaurante='{1} where (IdRestaurante='{2}')'", ltdd, lgnn, jee);
            MySqlCommand insert = new MySqlCommand(sql, conn);
            insert.ExecuteNonQuery();
            Finish();
        }
    }
}
