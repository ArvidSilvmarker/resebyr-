using System;
using System.Collections.Generic;

namespace Resebyrå
{
    public class Trip
    {
        public DateTime StartDate { get; set; }
        public int Duration { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public double Price { get; set; }
    }
}
