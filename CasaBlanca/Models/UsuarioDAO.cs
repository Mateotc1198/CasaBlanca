using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;
namespace CasaBlanca.Models
{
    public class UsuarioDAO
    {
        public static bool Registrar(Usuario usuario)
        {
            try
            {
                string conexion = ConfigurationManager.ConnectionStrings["CasaBlanca"].ConnectionString;
                System.Diagnostics.Debug.WriteLine("Conexion encontrada: " + conexion);

                using (SqlConnection con = new SqlConnection(conexion))
                {
                    con.Open();
                    System.Diagnostics.Debug.WriteLine("Conexion abierta correctamente");

                    SqlCommand checkCmd = new SqlCommand(
                        "SELECT COUNT(*) FROM Usuarios WHERE Usuario = @Usuario OR Correo = @Correo", con);

                    checkCmd.Parameters.AddWithValue("@Usuario", usuario.User);
                    checkCmd.Parameters.AddWithValue("@Correo", usuario.Correo);

                    // Se valida usuario y correo antes del insert para dar una respuesta controlada
                    // cuando existen duplicados y evitar depender solo del error de la base de datos.
                    int existe = (int)checkCmd.ExecuteScalar();
                    System.Diagnostics.Debug.WriteLine("Usuarios existentes encontrados: " + existe);

                    if (existe > 0)
                    {
                        System.Diagnostics.Debug.WriteLine("Usuario o correo ya existen");
                        return false;
                    }

                    SqlCommand cmd = new SqlCommand(
                        @"INSERT INTO Usuarios (Nombre, Apellido, Correo, Telefono, Usuario, Clave, EsAdmin)
          VALUES (@Nombre, @Apellido, @Correo, @Telefono, @Usuario, @Clave, 0)", con);

                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                    cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                    cmd.Parameters.AddWithValue("@Usuario", usuario.User);
                    // Nunca se guarda la contraseña en texto plano.
                    string claveHash = SeguridadHelper.GenerarHash(usuario.Clave);
                    cmd.Parameters.AddWithValue("@Clave", claveHash);

                    System.Diagnostics.Debug.WriteLine("Intentando insertar usuario: " + usuario.User);

                    int filas = cmd.ExecuteNonQuery();
                    System.Diagnostics.Debug.WriteLine("Filas afectadas: " + filas);

                    return filas > 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en Registrar: " + ex.Message);
                return false;
            }
        }

        public static Usuario ValidarLogin(string user, string clave)
        {
            try
            {
                string conexion = ConfigurationManager.ConnectionStrings["CasaBlanca"].ConnectionString;

                using (SqlConnection con = new SqlConnection(conexion))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(
                        "SELECT IdUsuario, Nombre, Usuario, EsAdmin FROM Usuarios WHERE Usuario = @Usuario AND Clave = @Clave", con);

                    cmd.Parameters.AddWithValue("@Usuario", user);
                    // El login compara hashes para que el valor real de la contraseña no viaje ni se persista.
                    string claveHash = SeguridadHelper.GenerarHash(clave);
                    cmd.Parameters.AddWithValue("@Clave", claveHash);

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        return new Usuario
                        {
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                            Nombre = dr["Nombre"].ToString(),
                            User = dr["Usuario"].ToString(),
                            EsAdmin = Convert.ToBoolean(dr["EsAdmin"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en Login: " + ex.Message);
            }

            return null;
        }
        public static List<Usuario> ListarUsuarios()
        {
            List<Usuario> lista = new List<Usuario>();

            try
            {
                string conexion = ConfigurationManager.ConnectionStrings["CasaBlanca"].ConnectionString;

                using (SqlConnection con = new SqlConnection(conexion))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(
                        "SELECT IdUsuario, Nombre, Apellido, Correo, Telefono, Usuario, EsAdmin FROM Usuarios ORDER BY IdUsuario DESC", con);

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        lista.Add(new Usuario
                        {
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                            Nombre = dr["Nombre"].ToString(),
                            Apellido = dr["Apellido"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            Telefono = dr["Telefono"].ToString(),
                            User = dr["Usuario"].ToString(),
                            EsAdmin = Convert.ToBoolean(dr["EsAdmin"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al listar usuarios: " + ex.Message);
            }

            return lista;
        }
        public static int ContarUsuarios()
        {
            try
            {
                string conexion = ConfigurationManager.ConnectionStrings["CasaBlanca"].ConnectionString;

                using (SqlConnection con = new SqlConnection(conexion))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Usuarios", con);
                    return (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al contar usuarios: " + ex.Message);
                return 0;
            }
        }
        public static bool EliminarUsuario(int idUsuario)
        {
            try
            {
                string conexion = ConfigurationManager.ConnectionStrings["CasaBlanca"].ConnectionString;

                using (SqlConnection con = new SqlConnection(conexion))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(
                        "DELETE FROM Usuarios WHERE IdUsuario = @IdUsuario", con);

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al eliminar usuario: " + ex.Message);
                return false;
            }
        }
    }
}
