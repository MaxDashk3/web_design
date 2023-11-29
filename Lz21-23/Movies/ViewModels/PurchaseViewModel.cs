using Movies.Models;

namespace Movies.ViewModels
{
    public class PurchaseViewModel
    {
        public PurchaseViewModel() { }

        public int PurchaseId { get; set; }
        public string Person { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }

        public List<TicketViewModel> Tickets { get; set; }

        public PurchaseViewModel(Purchase purchase)
        {
            PurchaseId = purchase.PurchaseId;
            Person = purchase.Person;
            Address = purchase.Address;
            Date = purchase.Date;

            if (purchase.Tickets != null) 
            {
                Tickets = purchase.Tickets.Select(t => new TicketViewModel(t)).ToList();
            }
        }
    }
}
