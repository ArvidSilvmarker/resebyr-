using System;
using System.Collections.Generic;
using System.Text;

namespace Resebyrå
{
    public enum BookingCode
    {
        Accepted,
        Unpaid,
        PaidFor,
        Denied,
        Unavailable
    }
    public class Booking
    {
        public Trip Trip { get; set; }
        public Traveler Traveler { get; set; }
        public BookingCode BookingCode { get; set; }
        public double Price { get; set; }
        public DateTime BookingDate { get; set; }

    }
}
