namespace Movies.Models
{
    public class Hall
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int Rows {  get; set; }
        public int SeatsPerRow { get; set; }

        public IEnumerable<Session> Sessions { get; set; }
    }
}
