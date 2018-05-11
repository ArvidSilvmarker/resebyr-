using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resebyrå;

namespace ResebyråTest
{
    [TestClass]
    public class TestAcceptedOrDenied
    {
        private TravelAgent travelAgent;
        private DateTime timeOfBooking;
        private Trip tripToBook;
        private Traveler diligentDave;
        private Traveler negligentNick;
        private Traveler firstTimeFred;
        private Traveler travelerTed;
        

        [TestInitialize]
        public void Init()
        {
            travelAgent = new TravelAgent(1000);
            timeOfBooking = new DateTime(2018,05,11);
            tripToBook = travelAgent.Trips.First(trip => trip.StartDate > timeOfBooking);

            diligentDave = new Traveler();
            diligentDave.Bookings.Add(new Booking { BookingCode = BookingCode.PaidFor });
            diligentDave.Bookings.Add(new Booking { BookingCode = BookingCode.PaidFor });
            diligentDave.Bookings.Add(new Booking { BookingCode = BookingCode.PaidFor });

            negligentNick = new Traveler();
            negligentNick.Bookings.Add(new Booking { BookingCode = BookingCode.PaidFor });
            negligentNick.Bookings.Add(new Booking { BookingCode = BookingCode.Unpaid });
            negligentNick.Bookings.Add(new Booking { BookingCode = BookingCode.Unpaid });
            negligentNick.Bookings.Add(new Booking { BookingCode = BookingCode.Unpaid });

            firstTimeFred = new Traveler();

            travelerTed = new Traveler();
            travelerTed.Bookings.Add(new Booking { BookingCode = BookingCode.PaidFor });
            travelerTed.Bookings.Add(new Booking { BookingCode = BookingCode.PaidFor });
            travelerTed.Bookings.Add(new Booking { BookingCode = BookingCode.PaidFor });
            travelerTed.Bookings.Add(new Booking { BookingCode = BookingCode.Unpaid });
            travelerTed.Bookings.Add(new Booking { BookingCode = BookingCode.Unpaid });
        }

        [TestMethod]
        public void CreateBooking_ThreePaid()
        {
            Booking actualBooking = travelAgent.CreateBooking(timeOfBooking, diligentDave, tripToBook);
            Assert.AreEqual(BookingCode.Accepted, actualBooking.BookingCode); 
        }

        [TestMethod]
        public void CreateBooking_ThreeUnpaid()
        {
            Booking actualBooking = travelAgent.CreateBooking(timeOfBooking, negligentNick, tripToBook);
            Assert.AreEqual(BookingCode.Denied, actualBooking.BookingCode);
        }

        [TestMethod]
        public void CreateBooking_FirstTimeTraveler()
        {
            Booking actualBooking = travelAgent.CreateBooking(timeOfBooking, firstTimeFred, tripToBook);
            Assert.AreEqual(BookingCode.Accepted, actualBooking.BookingCode);
        }

        [TestMethod]
        public void CreateBooking_TwoUnPaid()
        {
            Booking actualBooking = travelAgent.CreateBooking(timeOfBooking, travelerTed, tripToBook);
            Assert.AreEqual(BookingCode.Accepted, actualBooking.BookingCode);
        }
    }
}

