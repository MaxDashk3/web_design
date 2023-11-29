using Movies.Models;

namespace Movies.ViewModels
{
    public class MovieViewModel
    {
        public MovieViewModel() { }
        public int Id { get; set; }
        public string? Genre { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public int GenreId { get; set; }
        public int Year { get; set; }

        public List<SessionViewModel> Sessions { get; set; }

        public MovieViewModel(Movie movie)
        {
            Id = movie.Id;
            Title = movie.Title;
            Country = movie.Country;
            Year = movie.Year;
            GenreId = movie.GenreId;

            if (movie.Sessions != null)
            {
                Sessions = movie.Sessions.Select(s => new SessionViewModel(s)).ToList();
            }

            if (movie.Genre != null)
            {
                Genre = movie.Genre.Name;
            }
        }
    }
}
