﻿namespace Movies.Models
{
    public class Purchase
    {
        // ID покупки
        public int PurchaseId { get; set; }
        // ім'я й прізвище покупця
        public string Person { get; set; }
        // адреса покупця
        public string Address { get; set; }
        // ID книги
        public int TicketId { get; set; }
        // дата покупки
        public DateTime Date { get; set; }
    }
}
