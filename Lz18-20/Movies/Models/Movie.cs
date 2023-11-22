namespace Movies.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public int GenreId { get; set; }
        public int Year { get; set; } 

        public Genre Genre { get; set;}
        public IEnumerable<Session> Sessions { get; set;}
    }
}
