using System.Security.Cryptography;
using System.Text;

namespace CasaBlanca.Models
{
    public static class SeguridadHelper
    {
        public static string GenerarHash(string texto)
        {
            // Se centraliza la generación de hash para reutilizar exactamente la misma lógica
            // en registro y autenticación.
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(texto);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder resultado = new StringBuilder();

                foreach (byte b in hash)
                {
                    resultado.Append(b.ToString("x2"));
                }

                return resultado.ToString();
            }
        }
    }
}
