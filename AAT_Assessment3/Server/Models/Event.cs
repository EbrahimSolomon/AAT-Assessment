namespace AAT_Assessment3.Server.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
    }
}
