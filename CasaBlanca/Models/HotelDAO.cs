using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CasaBlanca.Models
{
    public class HotelDAO
    {
        public static List<Hotel> ListarHotelesActivos()
        {
            List<Hotel> lista = new List<Hotel>();

            try
            {
                string conexion = ConfigurationManager.ConnectionStrings["CasaBlanca"].ConnectionString;

                using (SqlConnection con = new SqlConnection(conexion))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(
                        "SELECT IdHotel, Nombre, Ciudad, PrecioPorNoche, CapacidadMaxima, Imagen, Activo FROM Hoteles WHERE Activo = 1 ORDER BY Nombre", con);

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        lista.Add(new Hotel
                        {
                            IdHotel = Convert.ToInt32(dr["IdHotel"]),
                            Nombre = dr["Nombre"].ToString(),
                            Ciudad = dr["Ciudad"].ToString(),
                            PrecioPorNoche = Convert.ToDecimal(dr["PrecioPorNoche"]),
                            CapacidadMaxima = Convert.ToInt32(dr["CapacidadMaxima"]),
                            Imagen = dr["Imagen"].ToString(),
                            Activo = Convert.ToBoolean(dr["Activo"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al listar hoteles: " + ex.Message);
            }

            return lista;
        }

        public static Hotel ObtenerPorId(int idHotel)
        {
            try
            {
                string conexion = ConfigurationManager.ConnectionStrings["CasaBlanca"].ConnectionString;

                using (SqlConnection con = new SqlConnection(conexion))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(
                        "SELECT IdHotel, Nombre, Ciudad, PrecioPorNoche, CapacidadMaxima, Imagen, Activo FROM Hoteles WHERE IdHotel = @IdHotel", con);

                    cmd.Parameters.AddWithValue("@IdHotel", idHotel);

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        return new Hotel
                        {
                            IdHotel = Convert.ToInt32(dr["IdHotel"]),
                            Nombre = dr["Nombre"].ToString(),
                            Ciudad = dr["Ciudad"].ToString(),
                            PrecioPorNoche = Convert.ToDecimal(dr["PrecioPorNoche"]),
                            CapacidadMaxima = Convert.ToInt32(dr["CapacidadMaxima"]),
                            Imagen = dr["Imagen"].ToString(),
                            Activo = Convert.ToBoolean(dr["Activo"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al obtener hotel: " + ex.Message);
            }

            return null;
        }
        public static bool CrearHotel(Hotel hotel)
        {
            try
            {
                string conexion = ConfigurationManager.ConnectionStrings["CasaBlanca"].ConnectionString;

                using (SqlConnection con = new SqlConnection(conexion))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"
                INSERT INTO Hoteles (Nombre, Ciudad, PrecioPorNoche, CapacidadMaxima, Imagen, Activo)
                VALUES (@Nombre, @Ciudad, @PrecioPorNoche, @CapacidadMaxima, @Imagen, @Activo)", con);

                    cmd.Parameters.AddWithValue("@Nombre", hotel.Nombre);
                    cmd.Parameters.AddWithValue("@Ciudad", hotel.Ciudad);
                    cmd.Parameters.AddWithValue("@PrecioPorNoche", hotel.PrecioPorNoche);
                    cmd.Parameters.AddWithValue("@CapacidadMaxima", hotel.CapacidadMaxima);
                    cmd.Parameters.AddWithValue("@Imagen", (object)(hotel.Imagen ?? ""));
                    cmd.Parameters.AddWithValue("@Activo", hotel.Activo);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al crear hotel: " + ex.Message);
                return false;
            }
        }

        public static bool ActualizarHotel(Hotel hotel)
        {
            try
            {
                string conexion = ConfigurationManager.ConnectionStrings["CasaBlanca"].ConnectionString;

                using (SqlConnection con = new SqlConnection(conexion))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"
                UPDATE Hoteles
                SET Nombre = @Nombre,
                    Ciudad = @Ciudad,
                    PrecioPorNoche = @PrecioPorNoche,
                    CapacidadMaxima = @CapacidadMaxima,
                    Imagen = @Imagen,
                    Activo = @Activo
                WHERE IdHotel = @IdHotel", con);

                    cmd.Parameters.AddWithValue("@IdHotel", hotel.IdHotel);
                    cmd.Parameters.AddWithValue("@Nombre", hotel.Nombre);
                    cmd.Parameters.AddWithValue("@Ciudad", hotel.Ciudad);
                    cmd.Parameters.AddWithValue("@PrecioPorNoche", hotel.PrecioPorNoche);
                    cmd.Parameters.AddWithValue("@CapacidadMaxima", hotel.CapacidadMaxima);
                    cmd.Parameters.AddWithValue("@Imagen", (object)(hotel.Imagen ?? ""));
                    cmd.Parameters.AddWithValue("@Activo", hotel.Activo);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al actualizar hotel: " + ex.Message);
                return false;
            }
        }

        public static List<Hotel> ListarTodos()
        {
            List<Hotel> lista = new List<Hotel>();

            try
            {
                string conexion = ConfigurationManager.ConnectionStrings["CasaBlanca"].ConnectionString;

                using (SqlConnection con = new SqlConnection(conexion))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(
                        "SELECT IdHotel, Nombre, Ciudad, PrecioPorNoche, CapacidadMaxima, Imagen, Activo FROM Hoteles ORDER BY IdHotel DESC", con);

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        lista.Add(new Hotel
                        {
                            IdHotel = Convert.ToInt32(dr["IdHotel"]),
                            Nombre = dr["Nombre"].ToString(),
                            Ciudad = dr["Ciudad"].ToString(),
                            PrecioPorNoche = Convert.ToDecimal(dr["PrecioPorNoche"]),
                            CapacidadMaxima = Convert.ToInt32(dr["CapacidadMaxima"]),
                            Imagen = dr["Imagen"].ToString(),
                            Activo = Convert.ToBoolean(dr["Activo"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al listar todos los hoteles: " + ex.Message);
            }

            return lista;
        }
    }
}  
