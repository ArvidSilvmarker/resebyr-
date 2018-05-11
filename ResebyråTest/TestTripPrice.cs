using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resebyrå;

namespace ResebyråTest
{
    [TestClass]
    public class TestTripPrice
    {
        private TravelAgent travelAgent;
        private Traveler diligentDave;
        private Traveler negligentNick;
        private Traveler firstTimeFred;
        private Traveler travelerTed;

        private Trip aprilTrip;
        private Trip mayTrip;
        private Trip juneTrip;
        private Trip julyTrip;

        private double singleTripPrice;
        private double dailyInterest;

        /* Det uppstod en fråga om hur vi skulle tolka Håkans regel för förseningsavgifter. 
         * "Ej betalat belopp för tidigare resor ska debiteras när personen anmäler sig till en ny resa och ingå i det totala beloppet för den resan.
         * Det ska även ingå en avgift på 5% per dag från föregående resa."
         * 
         * Efter varken Håkan eller produktägaren fanns tillgänglig valde jag att tolka den så här:
         * En resa = 1000 kr (tex.)
         * En resa + en tidigare obetald = 1000 + 1000 + dröjsmålsränta på 5 procent per dag sen förra resan avslutades, säg 13 => 188,56% av 1000 kr = ca 1886 kr => totalt ca 3886 kr
         * En resa + två tidigare obetalda = 1000 + 5072 (23 dagar dröjsmålsränta) + förseningsavgift 188,56% av 5072 kr = ca 9563 kr = totalt ca 15635 kr
         * Resenärerna betalar ingen ränta under själva resan.
         */



        [TestInitialize]
        public void Init()
        {
            singleTripPrice = 1000;
            travelAgent = new TravelAgent(singleTripPrice);
            dailyInterest = travelAgent.GetDailyInterest();

            aprilTrip = travelAgent.Trips.First(trip => trip.StartDate.Month == 04);
            mayTrip = travelAgent.Trips.First(trip => trip.StartDate.Month == 05);
            juneTrip = travelAgent.Trips.First(trip => trip.StartDate.Month == 06);
            julyTrip = travelAgent.Trips.First(trip => trip.StartDate.Month == 07);

            firstTimeFred = new Traveler();

            diligentDave = new Traveler();
            diligentDave.Bookings.Add(new Booking { BookingCode = BookingCode.PaidFor, Trip = aprilTrip });
            diligentDave.Bookings.Add(new Booking { BookingCode = BookingCode.PaidFor, Trip = mayTrip });
            diligentDave.Bookings.Add(new Booking { BookingCode = BookingCode.PaidFor, Trip = juneTrip });

            negligentNick = new Traveler();
        }

        [TestMethod]
        public void ExtraFee_FirstTimeTraveler()
        {
            Booking actualBooking = travelAgent.CreateBooking(new DateTime(2018, 06, 20), firstTimeFred, julyTrip);
            Assert.AreEqual(singleTripPrice, actualBooking.Price);
        }

        [TestMethod]
        public void ExtraFee_ThreePaid()
        {
            Booking actualBooking = travelAgent.CreateBooking(new DateTime(2018, 06, 20), diligentDave, julyTrip);
            Assert.AreEqual(singleTripPrice, actualBooking.Price);
        }

        [TestMethod]
        public void ExtraFee_OneUnpaid()
        {
            Booking firstBooking = travelAgent.CreateBooking(new DateTime(2018, 04, 20), negligentNick, mayTrip);
            firstBooking.BookingCode = BookingCode.Unpaid;

            Booking secondBooking = travelAgent.CreateBooking(new DateTime(2018, 05, 20), negligentNick, juneTrip);
            Double exptectedPrice = singleTripPrice + singleTripPrice * (1 + Math.Pow(dailyInterest, 13)); //13 dagar sedan senaste resa avslutades.                   
            Assert.AreEqual(exptectedPrice, secondBooking.Price);
        }

        [TestMethod]
        public void ExtraFee_TwoUnpaid()
        {
            Booking firstBooking = travelAgent.CreateBooking(new DateTime(2018, 04, 20), negligentNick, mayTrip);
            firstBooking.BookingCode = BookingCode.Unpaid;

            Booking secondBooking = travelAgent.CreateBooking(new DateTime(2018, 05, 20), negligentNick, juneTrip);
            secondBooking.BookingCode = BookingCode.Unpaid;

            Booking thirdBooking = travelAgent.CreateBooking(new DateTime(2018, 05, 20), negligentNick, juneTrip);
            Double junePrice = singleTripPrice + singleTripPrice * (1 + Math.Pow(dailyInterest, 23)); //23 dagar sedan senaste resa avslutades.
            Double exptectedPrice = singleTripPrice + junePrice * (1 + Math.Pow(dailyInterest, 13));
            Assert.AreEqual(exptectedPrice, thirdBooking.Price);
        }
    }
}
