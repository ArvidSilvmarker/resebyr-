using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resebyrå
{
    public class TravelAgent
    {
        private const int UnpaidTripLimit = 3;
        private const double DailyInterest = 1.05;
        public List<Trip> Trips { get; set; } = new List<Trip>();

        public TravelAgent(double pricePerTrip)
        {
            PreSeedTrips(pricePerTrip);
        }

        public Booking CreateBooking(DateTime dateOfBooking, Traveler traveler, Trip trip)
        {
            Booking booking = new Booking
            {
                Trip = trip,
                Traveler = traveler,
                BookingDate = dateOfBooking
            };

            if (CheckUnpaidTrips(traveler) >= UnpaidTripLimit)
                return DenyBooking(booking);

            if (!OpenForBooking(dateOfBooking, trip))
                return UnavailableBooking(booking);

            return AcceptBooking(booking);
        }

        private Booking UnavailableBooking(Booking booking)
        {
            booking.BookingCode = BookingCode.Unavailable;
            return booking;
        }

        private bool OpenForBooking(DateTime dateOfBooking, Trip trip)
        {
            if (dateOfBooking < trip.StartDate.AddDays(-5))
                return false;
            if (dateOfBooking > trip.StartDate.AddMonths(1))
                return false;
            return true;
        }

        private Booking DenyBooking(Booking booking)
        {
            booking.BookingCode = BookingCode.Denied;
            return booking;
        }

        private Booking AcceptBooking(Booking booking)
        {
            booking.BookingCode = BookingCode.Accepted;
            booking.Traveler.Bookings.Add(booking);
            booking.Trip.Bookings.Add(booking);
            booking.Price = CalculatePrice(booking);
            return booking;
        }

        private double CalculatePrice(Booking booking)
        {
            if (CheckUnpaidTrips(booking.Traveler) == 0)
                return booking.Trip.Price;

            var previousUnpaidBooking = GetPreviousUnpaidBooking(booking);
            return booking.Trip.Price + previousUnpaidBooking.Price * (1 + Math.Pow(DailyInterest, DaysWithInterest(booking, previousUnpaidBooking)));
        }

        private int DaysWithInterest(Booking booking, Booking previousUnpaidBooking)
        {
            return (booking.BookingDate - previousUnpaidBooking.BookingDate).Days;
        }

        private Booking GetPreviousUnpaidBooking(Booking booking)
        {
            booking.Traveler.Bookings
                .Where(b => b.BookingCode == BookingCode.Accepted)
                .Where(b => b.Trip.StartDate < booking.BookingDate)
                .ToList()
                .ForEach(b => b.BookingCode = BookingCode.Unpaid);

            List<Booking> unpaidBookings = booking.Traveler.Bookings.Where(b => b.BookingCode == BookingCode.Unpaid).ToList();
            var previousUnPaidBooking = unpaidBookings.First(b => b.BookingDate < booking.BookingDate);
            foreach (var unpaidBooking in unpaidBookings)
            {
                if (unpaidBooking.BookingDate > previousUnPaidBooking.BookingDate)
                    previousUnPaidBooking = unpaidBooking;
            }

            return previousUnPaidBooking;
        }

        private int CheckUnpaidTrips(Traveler traveler)
        {
            int NumberOfUnpaid = 0;
            foreach (var booking in traveler.Bookings)
            {
                if (booking.BookingCode == BookingCode.Unpaid)
                    NumberOfUnpaid++;

            }
            return NumberOfUnpaid;
        }

        private void PreSeedTrips(double pricePerTrip)
        {
            int tripDuration = 7;

            Trips.Add(new Trip { Price = pricePerTrip, StartDate = new DateTime(2018, 02, 01), Duration = tripDuration });
            Trips.Add(new Trip { Price = pricePerTrip, StartDate = new DateTime(2018, 03, 01), Duration = tripDuration });
            Trips.Add(new Trip { Price = pricePerTrip, StartDate = new DateTime(2018, 04, 01), Duration = tripDuration });
            Trips.Add(new Trip { Price = pricePerTrip, StartDate = new DateTime(2018, 05, 01), Duration = tripDuration });
            Trips.Add(new Trip { Price = pricePerTrip, StartDate = new DateTime(2018, 06, 01), Duration = tripDuration });
            Trips.Add(new Trip { Price = pricePerTrip, StartDate = new DateTime(2018, 07, 01), Duration = tripDuration });
            Trips.Add(new Trip { Price = pricePerTrip, StartDate = new DateTime(2018, 08, 01), Duration = tripDuration });
            Trips.Add(new Trip { Price = pricePerTrip, StartDate = new DateTime(2018, 09, 01), Duration = tripDuration });
            Trips.Add(new Trip { Price = pricePerTrip, StartDate = new DateTime(2018, 10, 01), Duration = tripDuration });
            Trips.Add(new Trip { Price = pricePerTrip, StartDate = new DateTime(2018, 11, 01), Duration = tripDuration });
            Trips.Add(new Trip { Price = pricePerTrip, StartDate = new DateTime(2018, 12, 01), Duration = tripDuration });
        }

        public double GetDailyInterest()
        {
            return DailyInterest;
        }
    }
}
