using Castle.Core.Logging;
using Castle.Core.Resource;
using Microsoft.Extensions.Logging;
using Moq;
using SimplyFly_Project.DTO;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using SimplyFly_Project.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyFly_Test
{
    internal class BookingServiceTest
    {
        private BookingService _bookingService;
        private Mock<IRepository<int, Booking>> _mockBookingRepository;
        private Mock<IRepository<string, Flight>> _mockFlightRepository;
        private Mock<IRepository<int, PassengerBooking>> _mockPassengerBookingRepository;
        private Mock<ISeatDetailRepository> _mockSeatDetailRepository;
        private Mock<IPassengerBookingRepository> _mockPassengerBookingsRepository;
        private Mock<IBookingRepository> _mockBookingsRepository;
        private Mock<IRepository<int, Payment>> _mockPaymentRepository;
        private Mock<IRepository<int, Schedule>> _mockScheduleRepository;
        private Mock<IRepository<string, SeatDetail>> _mockSeatRepository;
        private Mock<IRepository<int, CancelledBooking>> _mockCancelBookingRepository;
        private Mock<ILogger<BookingService>> _mockLogger;

        [SetUp]
        public void SetUp()
        {
            _mockBookingRepository = new Mock<IRepository<int, Booking>>();
            _mockFlightRepository = new Mock<IRepository<string, Flight>>();
            _mockPassengerBookingsRepository = new Mock<IPassengerBookingRepository>();
            _mockPassengerBookingRepository = new Mock<IRepository<int, PassengerBooking>>();
            _mockScheduleRepository = new Mock<IRepository<int, Schedule>>();
            _mockPaymentRepository = new Mock<IRepository<int, Payment>>();
            _mockSeatDetailRepository = new Mock<ISeatDetailRepository>();
            _mockBookingsRepository = new Mock<IBookingRepository>();
            _mockSeatRepository = new Mock<IRepository<string, SeatDetail>>();
            _mockCancelBookingRepository = new Mock<IRepository<int, CancelledBooking>>();
            _mockLogger = new Mock<ILogger<BookingService>>();

            _bookingService = new BookingService(_mockBookingRepository.Object, _mockFlightRepository.Object, _mockPassengerBookingRepository.Object, _mockScheduleRepository.Object, _mockPaymentRepository.Object,_mockSeatDetailRepository.Object,_mockPassengerBookingsRepository.Object,_mockBookingsRepository.Object, _mockSeatRepository.Object, _mockCancelBookingRepository.Object,_mockLogger.Object);
        }

        [Test]
        [Order(3)]
        public async Task CreateBooking_Validate_Returns_True_Test()
        {
            //arrange
            var bookingRequestDTO = new BookingRequestDTO
            {
                ScheduleId = 1,
                CustomerId = 1,
                SelectedSeats = new List<string> { "FLI009SE01", "FLI009SE02" },
                PassengerIds = new List<int> { 1, 2 },
            };

            Payment payment = new Payment { Amount = 4000, PaymentDate = DateTime.Now, PaymentStatus = "Successful" };
            var flight = new Flight { FlightNumber = "FLI009", BasePrice = 2000 };

            _mockFlightRepository.Setup(repo => repo.GetAsync("FLI009")).ReturnsAsync(flight);
            _mockPassengerBookingsRepository.Setup(repo => repo.CheckSeatsAvailbilityAsync(1, It.IsAny<List<string>>())).ReturnsAsync(true);
            _mockSeatDetailRepository.Setup(repo => repo.GetSeatDetailsAsync(bookingRequestDTO.SelectedSeats)).ReturnsAsync(new List<SeatDetail> { new SeatDetail { SeatNo = "FLI009SE01", SeatClass = "Economy" }, new SeatDetail { SeatNo = "FLI009SE02", SeatClass = "Business" } });
            _mockScheduleRepository.Setup(repo => repo.GetAsync(1)).ReturnsAsync(new Schedule { FlightNumber = "FLI009", Departure = DateTime.Now.AddDays(3), Arrival = DateTime.Now.AddDays(2), RouteId = 1 });
            _mockBookingRepository.Setup(repo => repo.Add(It.IsAny<Booking>())).ReturnsAsync(new Booking { BookingId = 1, ScheduleId = 1, CustomerId = 1, BookingTime = DateTime.Now, TotalPrice = 4000 });
            _mockPassengerBookingRepository.Setup(repo => repo.Add(It.IsAny<PassengerBooking>())).ReturnsAsync(new PassengerBooking { PassengerBookingId = 1, BookingId = 1, PassengerId = 1, SeatNumber = "FLI009SE01" });
            _mockPaymentRepository.Setup(repo => repo.Add(It.IsAny<Payment>())).ReturnsAsync(new Payment { PaymentId = 1, Amount = 4000, PaymentDate = DateTime.Now, PaymentStatus = "Successful" });

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await _bookingService.CreateBookingAsync(bookingRequestDTO));
            Assert.AreEqual("Invalid seat selection, please try selecting correctly.", exception.Message);

        }


        [Test]
        public async Task GetAllBookings_Test()
        {
            //arrange
            var bookings = new List<Booking>()
            {
                new Booking {BookingId = 1, ScheduleId = 1, CustomerId = 1, BookingTime = DateTime.Now, TotalPrice = 4000}
            };

            _mockBookingRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(bookings);   

            //act
            var result = await _bookingService.GetAllBookingsAsync();

            //assert
            Assert.IsNotNull(result);
        }


        [Test]
        public async Task GetBookingByIdAsync_ExistingBookingId_Validate_Test()
        {
            // Arrange
            var bookingId = 1;
            var booking = new Booking
            {
                BookingId = bookingId,
                ScheduleId = 1,
                CustomerId = 1,
                BookingTime = DateTime.Now,
                TotalPrice = 4000
            };

            _mockBookingRepository.Setup(repo => repo.GetAsync(bookingId)).ReturnsAsync(booking);

            // Act
            var result = await _bookingService.GetBookingByIdAsync(bookingId);

            // Assert
            Assert.AreEqual(booking, result);
        }



        [Test]
        public async Task CancelBookingAsync_ExistingBookingId_ReturnsCancelledBooking()
        {
            // Arrange
            var bookingId = 1;
            var scheduleId = 1;

            var booking = new Booking { BookingId = 1, ScheduleId = 1, CustomerId = 1, BookingTime = DateTime.Now, TotalPrice = 200 };
            var payment = new Payment { PaymentId = 1, Amount = 4000, PaymentDate = DateTime.Now, PaymentStatus = "Successful" };
            _mockBookingRepository.Setup(repo => repo.GetAsync(bookingId)).ReturnsAsync(booking);
            _mockBookingRepository.Setup(repo => repo.GetAsync(scheduleId)).ReturnsAsync(booking);
            _mockPaymentRepository.Setup(repo => repo.GetAsync(booking.PaymentId)).ReturnsAsync(payment);
            _mockPassengerBookingsRepository.Setup(repo => repo.GetPassengerBookingAsync(bookingId)).ReturnsAsync(new List<PassengerBooking> { new PassengerBooking { PassengerBookingId = 1, BookingId = 1, PassengerId = 1, SeatNumber = "FLI009SE01" } });
            _mockBookingRepository.Setup(repo => repo.Delete(bookingId)).ReturnsAsync(booking);
            _mockPaymentRepository.Setup(repo => repo.Delete(bookingId)).ReturnsAsync(payment);
            _mockSeatDetailRepository.Setup(repo => repo.UpdateSeatDetailsAsync(It.IsAny<IEnumerable<SeatDetail>>())).Returns(Task.CompletedTask);

            // Act
            var result = await _bookingService.CancelBookingAsync(bookingId);

            // Assert
            Assert.That(result, Is.EqualTo(booking));
        }


        [Test]
        public async Task GetCustomerBookingsAsync_ExistingCustomerId_ReturnsListOfBookings_Test()
        {
            // Arrange
            var customerId = 1;
            var bookings = new List<Booking> { new Booking { BookingId = 1, ScheduleId = 1, CustomerId = customerId, BookingTime = DateTime.Now, TotalPrice = 4000 } };
            _mockBookingsRepository.Setup(repo => repo.GetBookingsByCustomerIdAsync(customerId)).ReturnsAsync(bookings);

            // Act
            var result = await _bookingService.GetUserBookingsAsync(customerId);

            // Assert
            Assert.That(result, Is.EqualTo(bookings));
        }

        [Test]
        public void CalculateTotalPrice_Validate_Returns_TotalPrice_Test()
        {
            // Arrange
            var numberOfSeats = 2;
            var flight = new Flight { BasePrice = 2000 };

            // Act
            var totalPrice = _bookingService.CalculateTotalPrice(numberOfSeats, flight);

            // Assert
            Assert.AreEqual(4000, totalPrice);
        }


        [Test]
        public async Task RequestRefundAsync_ValidBookingId_ReturnsTrue_Test()
        {
            // arrange
            var bookingId = 1;
            var cancelBookingId = 1;
            var booking = new Booking { BookingId = bookingId };
            var payment = new Payment { PaymentId = 1, Amount = 4000, PaymentDate = DateTime.Now, PaymentStatus = "Successful" };

            _mockBookingRepository.Setup(repo => repo.GetAsync(bookingId)).ReturnsAsync(booking);
            _mockPaymentRepository.Setup(repo => repo.GetAsync(booking.PaymentId)).ReturnsAsync(payment);
            _mockPaymentRepository.Setup(repo => repo.Update(payment)).ReturnsAsync(payment);

            // act
            var result = await _bookingService.RequestRefundAsync(bookingId,cancelBookingId);

            // assert
            Assert.IsTrue(result);
        }


        [Test]
        public async Task GetBookingBy_Schedule_Test()
        {
            //arrange
            var bookingRequest = new BookingRequestDTO
            {
                ScheduleId = 1,
                CustomerId = 1,
                SelectedSeats = new List<string> { "FLI009SE01", "FLI009SE02" },
                PassengerIds = new List<int> { 1, 2 },
                Price = 4000
            };

            Payment payment = new Payment { PaymentId = 1 ,Amount = 4000, PaymentDate = DateTime.Now, PaymentStatus = "Successful" };
            var flight = new Flight { FlightNumber = "FLI009", BasePrice = 2000 };

            

            _mockFlightRepository.Setup(repo => repo.GetAsync("FLI009")).ReturnsAsync(flight);
            _mockPassengerBookingsRepository.Setup(repo => repo.CheckSeatsAvailbilityAsync(1, It.IsAny<List<string>>())).ReturnsAsync(true);
            _mockSeatDetailRepository.Setup(repo => repo.GetSeatDetailsAsync(bookingRequest.SelectedSeats)).ReturnsAsync(new List<SeatDetail> { new SeatDetail { SeatNo = "FLI009SE01", SeatClass = "Economy" }, new SeatDetail { SeatNo = "FLI009SE02", SeatClass = "Business" } });
            _mockScheduleRepository.Setup(repo => repo.GetAsync(1)).ReturnsAsync(new Schedule { ScheduleId = 1 ,FlightNumber = "FLI009", Departure = DateTime.Now.AddDays(3), Arrival = DateTime.Now.AddDays(2), RouteId = 1 });
            _mockBookingRepository.Setup(repo => repo.Add(It.IsAny<Booking>())).ReturnsAsync(new Booking { BookingId = 1, ScheduleId = 1, CustomerId = 1, BookingTime = DateTime.Now, TotalPrice = 4000 });
            _mockPassengerBookingRepository.Setup(repo => repo.Add(It.IsAny<PassengerBooking>())).ReturnsAsync(new PassengerBooking { PassengerBookingId = 1, BookingId = 1, PassengerId = 1, SeatNumber = "FLI009SE01" });
            _mockPaymentRepository.Setup(repo => repo.Add(It.IsAny<Payment>())).ReturnsAsync(new Payment { PaymentId = 1, Amount = 4000, PaymentDate = DateTime.Now, PaymentStatus = "Successful" });
            _mockBookingRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(new List<Booking> { new Booking { BookingId = 1, ScheduleId = 1, CustomerId = 1, BookingTime = DateTime.Now, TotalPrice = 4000 } });

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await _bookingService.CreateBookingAsync(bookingRequest));
            Assert.AreEqual("Invalid seat selection, please try selecting correctly.", exception.Message);
        }


        [Test]
        public async Task GetBookedSeatBySchedule_ValidScheduleId_ReturnsBookedSeats()
        {
            // arrange
            var scheduleId = 1;
            var passengerBookings = new List<PassengerBooking>
            {
                new PassengerBooking { PassengerBookingId = 1,Booking = new Booking{ScheduleId = 1}, BookingId = 1, SeatNumber = "FLI009SE01" },
                new PassengerBooking { PassengerBookingId = 2,Booking = new Booking{ScheduleId = 1}, BookingId = 1, SeatNumber = "FLI009SE02" }
            };

            _mockPassengerBookingRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(passengerBookings);

            // act
            var result = await _bookingService.GetBookedSeatBySchedule(scheduleId);

            // assert
            Assert.That(result, Is.EqualTo(new List<string> { "FLI009SE01", "FLI009SE02" }));
        }


        [Test]
        public async Task GetBookingByFlight_Test()
        {
            // arrange
            string flightNumber = "FLI009";
            var mockBookings = new List<Booking>
            {
                new Booking { BookingId = 1, Schedule = new Schedule { FlightNumber = flightNumber } },
                new Booking { BookingId = 2, Schedule = new Schedule { FlightNumber = flightNumber } }
            };

            _mockBookingRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(mockBookings);

            // act
            var result = await _bookingService.GetBookingByFlight(flightNumber);

            // assert
            Assert.IsNotNull(result);
        }


        [Test]
        public async Task Get_BookedSeat_By_Schedule_Test()
        {
            // arrange
            int scheduleId = 1;
            var mockPassengerBookings = new List<PassengerBooking>
            {
                new PassengerBooking { BookingId = 1, Booking = new Booking { ScheduleId = scheduleId }, SeatNumber = "FLI009SE01" },
                new PassengerBooking { BookingId = 2, Booking = new Booking { ScheduleId = scheduleId }, SeatNumber = "FLI009SE02" }

            };

            _mockPassengerBookingRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(mockPassengerBookings);

            // act
            var result = await _bookingService.GetBookedSeatBySchedule(scheduleId);

            // assert
            Assert.IsNotNull(result);
            
        }


        [Test]
        public async Task Get_Bookings_By_CustomerId_Test()
        {
            // arrange
            int customerId = 1;
            var mockPassengerBookings = new List<PassengerBooking>
            {
                new PassengerBooking { PassengerBookingId = 1, Booking = new Booking { CustomerId = customerId }, SeatNumber = "FLI009SE01" },
                new PassengerBooking { PassengerBookingId = 2, Booking = new Booking { CustomerId = customerId }, SeatNumber = "FLI009SE02" }
            };

            _mockPassengerBookingRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(mockPassengerBookings);

            // act
            var result = await _bookingService.GetBookingsByCustomerId(customerId);

            // assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Cancel_BookingBy_Passenger()
        {
            // arrange
            int passengerId = 1;
            var mockPassengerBooking = new PassengerBooking
            {
                PassengerBookingId = passengerId,
                Booking = new Booking { BookingId = 1, ScheduleId = 1 },
                SeatNumber = "E1"
            };

            _mockPassengerBookingRepository.Setup(repo => repo.GetAsync(passengerId)).ReturnsAsync(mockPassengerBooking);
            _mockPassengerBookingRepository.Setup(repo => repo.Delete(passengerId)).ReturnsAsync(mockPassengerBooking);

  

            // Act and Assert
            Assert.ThrowsAsync<NoSuchPassengerException>(async () => await _bookingService.CancelBookingByPassenger(passengerId));
        }

    }
}
