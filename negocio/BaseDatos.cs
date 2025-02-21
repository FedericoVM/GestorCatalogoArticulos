using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace negocio
{
    internal class BaseDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        public SqlDataReader Lector { get; set; }

        public BaseDatos()
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS;database = CATALOGO_DB; integrated security = true");
            comando = new SqlCommand();
        }

        public void setConsulta( string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        public void ejecutarConsulta()
        {
            comando.Connection = conexion;

            try
            {
                comando.Connection.Open();
                Lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void cerrarConexion()
        {
            if (lector != null)
            {
                lector.Close();
            }

            conexion.Close();
        }

    }
}
