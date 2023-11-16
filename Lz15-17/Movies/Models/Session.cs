namespace Movies.Models
{
    public class Session
    {
        public int Id { get; set; }
        public int HallId { get; set; }
        public int MovieId { get; set; }
        public DateTime TimeDate { get; set; }
        public int Price { get; set; }
        
        public Hall Hall { get; set; }
        public Movie Movie { get; set; }
        public IEnumerable<Ticket> Tickets { get; set; }
    }
}
