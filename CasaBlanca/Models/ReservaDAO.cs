using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
namespace CasaBlanca.Models
{
    public class ReservaDAO
    {
        public static bool Guardar(Reserva reserva, int? idUsuario = null)
        {
            try
            {
                string conexion = ConfigurationManager.ConnectionStrings["CasaBlanca"].ConnectionString;

                using (SqlConnection con = new SqlConnection(conexion))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"
    INSERT INTO Reservas
    (IdUsuario, IdHotel, Destino, FechaEntrada, FechaSalida, Huespedes, Estado, PrecioPorNoche, Noches, Total)
    VALUES
    (@IdUsuario, @IdHotel, @Destino, @FechaEntrada, @FechaSalida, @Huespedes, @Estado, @PrecioPorNoche, @Noches, @Total)", con);
                    // Huespedes llega como texto desde la vista para integrarse con los controles del formulario;
                    // aquí se normaliza antes de persistir.
                    int huespedes = 0;
                    int.TryParse(reserva.Huespedes, out huespedes);
                    cmd.Parameters.AddWithValue("@IdUsuario", (object)idUsuario ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdHotel", reserva.IdHotel);
                    cmd.Parameters.AddWithValue("@Destino", reserva.Destino);
                    cmd.Parameters.AddWithValue("@FechaEntrada", reserva.Entrada);
                    cmd.Parameters.AddWithValue("@FechaSalida", reserva.Salida);
                    cmd.Parameters.AddWithValue("@Huespedes", huespedes);
                    cmd.Parameters.AddWithValue("@Estado", "Pendiente");
                    cmd.Parameters.AddWithValue("@PrecioPorNoche", reserva.PrecioPorNoche);
                    cmd.Parameters.AddWithValue("@Noches", reserva.Noches);
                    cmd.Parameters.AddWithValue("@Total", reserva.Total);


                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en Guardar reserva: " + ex.Message);
                return false;
            }
        }
        public static List<Reserva> ListarReservas()
        {
            List<Reserva> lista = new List<Reserva>();

            try
            {
                string conexion = ConfigurationManager.ConnectionStrings["CasaBlanca"].ConnectionString;

                using (SqlConnection con = new SqlConnection(conexion))
                {
                    con.Open();

                    // Se usan LEFT JOIN para conservar reservas históricas incluso si el usuario o el hotel
                    // ya no están disponibles del mismo modo en el catálogo.
                    SqlCommand cmd = new SqlCommand(@"
    SELECT 
        r.IdReserva,
        r.IdUsuario,
        r.IdHotel,
        r.Destino,
        r.FechaEntrada,
        r.FechaSalida,
        r.Huespedes,
        r.Estado,
        r.PrecioPorNoche,
        r.Noches,
        r.Total,
        u.Usuario AS NombreUsuario,
        u.Correo AS CorreoUsuario,
        h.Nombre AS NombreHotel,
        h.Imagen AS ImagenHotel
    FROM Reservas r
    LEFT JOIN Usuarios u ON r.IdUsuario = u.IdUsuario
    LEFT JOIN Hoteles h ON r.IdHotel = h.IdHotel
    ORDER BY r.IdReserva DESC", con);

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        lista.Add(new Reserva
                        {
                            IdReserva = Convert.ToInt32(dr["IdReserva"]),
                            IdUsuario = dr["IdUsuario"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdUsuario"]),
                            Destino = dr["Destino"].ToString(),
                            Entrada = Convert.ToDateTime(dr["FechaEntrada"]),
                            Salida = Convert.ToDateTime(dr["FechaSalida"]),
                            Huespedes = dr["Huespedes"].ToString(),
                            Estado = dr["Estado"].ToString(),
                            NombreUsuario = dr["NombreUsuario"] == DBNull.Value ? "" : dr["NombreUsuario"].ToString(),
                            CorreoUsuario = dr["CorreoUsuario"] == DBNull.Value ? "" : dr["CorreoUsuario"].ToString(),
                            PrecioPorNoche = dr["PrecioPorNoche"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["PrecioPorNoche"]),
                            Noches = dr["Noches"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Noches"]),
                            Total = dr["Total"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["Total"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al listar reservas: " + ex.Message);
            }

            return lista;
        }
        public static List<Reserva> ListarReservasPorUsuario(int idUsuario)
        {
            List<Reserva> lista = new List<Reserva>();

            try
            {
                string conexion = ConfigurationManager.ConnectionStrings["CasaBlanca"].ConnectionString;

                using (SqlConnection con = new SqlConnection(conexion))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"
    SELECT 
        r.IdReserva,
        r.IdUsuario,
        r.IdHotel,
        r.Destino,
        r.FechaEntrada,
        r.FechaSalida,
        r.Huespedes,
        r.Estado,
        r.PrecioPorNoche,
        r.Noches,
        r.Total,
        u.Usuario AS NombreUsuario,
        u.Correo AS CorreoUsuario,
        h.Nombre AS NombreHotel,
        h.Imagen AS ImagenHotel
    FROM Reservas r
    LEFT JOIN Usuarios u ON r.IdUsuario = u.IdUsuario
    LEFT JOIN Hoteles h ON r.IdHotel = h.IdHotel
    WHERE r.IdUsuario = @IdUsuario
    ORDER BY r.IdReserva DESC", con);

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        lista.Add(new Reserva
                        {
                            IdReserva = Convert.ToInt32(dr["IdReserva"]),
                            IdUsuario = dr["IdUsuario"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdUsuario"]),
                            IdHotel = dr["IdHotel"] == DBNull.Value ? 0 : Convert.ToInt32(dr["IdHotel"]),
                            NombreHotel = dr["NombreHotel"] == DBNull.Value ? "" : dr["NombreHotel"].ToString(),
                            Destino = dr["Destino"].ToString(),
                            Entrada = Convert.ToDateTime(dr["FechaEntrada"]),
                            Salida = Convert.ToDateTime(dr["FechaSalida"]),
                            Huespedes = dr["Huespedes"].ToString(),
                            Estado = dr["Estado"].ToString(),
                            PrecioPorNoche = dr["PrecioPorNoche"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["PrecioPorNoche"]),
                            Noches = dr["Noches"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Noches"]),
                            Total = dr["Total"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["Total"]),
                            ImagenHotel = dr["ImagenHotel"] == DBNull.Value ? "" : dr["ImagenHotel"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al listar reservas del usuario: " + ex.Message);
            }

            return lista;
        }
        public static bool ActualizarEstado(int idReserva, string estado)
        {
            try
            {
                string conexion = ConfigurationManager.ConnectionStrings["CasaBlanca"].ConnectionString;

                using (SqlConnection con = new SqlConnection(conexion))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Reservas SET Estado = @Estado WHERE IdReserva = @IdReserva", con);

                    cmd.Parameters.AddWithValue("@Estado", estado);
                    cmd.Parameters.AddWithValue("@IdReserva", idReserva);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al actualizar estado: " + ex.Message);
                return false;
            }
        }
        public static int ContarReservas()
        {
            try
            {
                string conexion = ConfigurationManager.ConnectionStrings["CasaBlanca"].ConnectionString;

                using (SqlConnection con = new SqlConnection(conexion))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Reservas", con);
                    return (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al contar reservas: " + ex.Message);
                return 0;
            }
        }

        public static int ContarReservasPorEstado(string estado)
        {
            try
            {
                string conexion = ConfigurationManager.ConnectionStrings["CasaBlanca"].ConnectionString;

                using (SqlConnection con = new SqlConnection(conexion))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Reservas WHERE Estado = @Estado", con);
                    cmd.Parameters.AddWithValue("@Estado", estado);

                    return (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al contar reservas por estado: " + ex.Message);
                return 0;
            }
        }
        public static bool EliminarReserva(int idReserva)
        {
            try
            {
                string conexion = ConfigurationManager.ConnectionStrings["CasaBlanca"].ConnectionString;

                using (SqlConnection con = new SqlConnection(conexion))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(
                        "DELETE FROM Reservas WHERE IdReserva = @IdReserva", con);

                    cmd.Parameters.AddWithValue("@IdReserva", idReserva);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al eliminar reserva: " + ex.Message);
                return false;
            }
        }
        public static bool CancelarReservaUsuario(int idReserva, int idUsuario)
        {
            try
            {
                string conexion = ConfigurationManager.ConnectionStrings["CasaBlanca"].ConnectionString;

                using (SqlConnection con = new SqlConnection(conexion))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"
                UPDATE Reservas
                SET Estado = 'Cancelada'
                WHERE IdReserva = @IdReserva
                  AND IdUsuario = @IdUsuario
                  AND Estado = 'Pendiente'", con);

                    // Esta condición evita que un usuario modifique reservas ajenas o ya procesadas.
                    cmd.Parameters.AddWithValue("@IdReserva", idReserva);
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al cancelar reserva del usuario: " + ex.Message);
                return false;
            }
        }
    }
}
