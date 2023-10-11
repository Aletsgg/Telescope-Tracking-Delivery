
using Microsoft.Data.SqlClient;
using System.Data;
using Telescope_Traking_Delivery.Models;







namespace Telescope_Traking_Delivery.Logica
{
    public class LO_Usuarios
    {


        public Usuario EncontrarUsuario(string Correo, string Clave)
        {
            Usuario objeto = new Usuario();

            using (SqlConnection conexion = new SqlConnection("server=DESKTOP-AFQ1FNV\\SQLEXPRESS; database=DB_TRACKER2; Encrypt=false; integrated security=true; Trusted_Connection=true;"))

            //using (SqlConnection conexion = new SqlConnection("server=189.245.18.29; database=DB_TRACKER; user id=tracker; password=Nbio2023; Encrypt=false; Trusted_Connection=false;"))

            //using (SqlConnection conexion = new SqlConnection("server=192.168.2.190; database=DB_TRACKER; user id=tracker; password=Nbio2023; Encrypt=false; Trusted_Connection=false;"))

            {

                string query = "select * from Usuario where Correo = @Correo and Clave = @Clave ;";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Correo", Correo);
                cmd.Parameters.AddWithValue("@Clave", Clave);

                cmd.CommandType = CommandType.Text;


                conexion.Open();


                using (SqlDataReader dr = cmd.ExecuteReader())
                {

                    while (dr.Read())
                    {

                        objeto = new Usuario()
                        {
                            IdUsuario = int.Parse(dr["IdUsuario"].ToString()),
                            Nombre = dr["Nombre"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            Clave = dr["Clave"].ToString(),
                            UrlFoto = dr["UrlFoto"].ToString(),
                            IdRol = int.Parse(dr["IdRol"].ToString())

                        };
                    }

                }
            }
            return objeto;

        }




    }
}