namespace Movies.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; }
        public DateTime DateTime { get; set; }
        public string Country { get; set; }
        public int SeatRow { get; set; }
        public int SeatNum { get; set; }
        public int Price { get; set;}
    }
}
