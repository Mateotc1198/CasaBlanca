namespace CasaBlanca.Models
{
    public class AdminDashboard
    {
        public int TotalUsuarios { get; set; }
        public int TotalReservas { get; set; }
        public int ReservasPendientes { get; set; }
        public int ReservasConfirmadas { get; set; }
        public int ReservasCanceladas { get; set; }
    }
}