namespace Movies.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int? PurchaseId { get; set; }
        public int SessionId { get; set; }
        public int SeatRow { get; set; }
        public int SeatNum { get; set; }

        public Purchase? Purchase { get; set; }
        public Session Session { get; set; }

    }
}
