using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resebyrå;

namespace ResebyråTest
{
    [TestClass]
    public class TestBookingDate
    {
        private TravelAgent travelAgent;
        private DateTime timeOfBooking;
        private Trip tripToBook;
        private Traveler traveler;
        private DateTime goodTimeOfBooking;
        private DateTime badTimeOfBooking1;
        private DateTime badTimeOfBooking2;


        [TestInitialize]
        public void Init()
        {
            travelAgent = new TravelAgent(1000);
            goodTimeOfBooking = new DateTime(2018, 05, 11);
            badTimeOfBooking1 = new DateTime(2018, 05, 27);
            badTimeOfBooking2 = new DateTime(2018, 04, 11);
            tripToBook = travelAgent.Trips.First(trip => trip.StartDate > timeOfBooking);
            traveler = new Traveler();

        }

        [TestMethod]
        public void CreateBooking_InTime()
        {
            Booking actualBooking = travelAgent.CreateBooking(goodTimeOfBooking, traveler, tripToBook);
            Assert.AreEqual(BookingCode.Accepted, actualBooking.BookingCode);
        }

        [TestMethod]
        public void CreateBooking_After()
        {
            Booking actualBooking = travelAgent.CreateBooking(badTimeOfBooking1, traveler, tripToBook);
            Assert.AreEqual(BookingCode.Unavailable, actualBooking.BookingCode);
        }

        [TestMethod]
        public void CreateBooking_Before()
        {
            Booking actualBooking = travelAgent.CreateBooking(badTimeOfBooking2, traveler, tripToBook);
            Assert.AreEqual(BookingCode.Unavailable, actualBooking.BookingCode);
        }

    }
}