using System;
using MySql.Data.MySqlClient;

namespace TFEatery.MySQLconector
{
    public class DBcon
    {
        private MySqlConnection Conexion;

        public DBcon()
        {
            MySqlConnectionStringBuilder con = new MySqlConnectionStringBuilder();
            con.Server = "mysql-10951-0.cloudclusters.net";
            con.Port = 10951;
            con.Database = "TapFood";
            con.UserID = "curecu";
            con.Password = "curecu123";

            Conexion = new MySqlConnection(con.ToString());

            Conectar();

        }

        private bool Conectar()
        {
            try
            {
                Conexion.Open();
                Error = "";
                return true;
            }
            catch (MySqlException ex)
            {
                Error = ex.Message;
                return false;
            }
        }

        public string Error { get; set; }


        public bool Comando(string command)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(command, Conexion);
                cmd.ExecuteNonQuery();
                Error = "";
                return true;
            }

            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
                throw;
            }
        }

        public object Consulta(string Consulta)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(Consulta, Conexion);
                MySqlDataReader dr = cmd.ExecuteReader();
                Error = "";
                return dr;

            }

            catch (Exception ex)
            {
                Error = ex.Message;
                return null;
            }

        }

        ~DBcon()
        {
            if (Conexion.State == System.Data.ConnectionState.Open)
            {
                Conexion.Close();
            }

        }
    }
}

