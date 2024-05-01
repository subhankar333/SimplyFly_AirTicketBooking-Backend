using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SimplyFly_Project.Context;
using SimplyFly_Project.DTO;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using SimplyFly_Project.Repositories;
using SimplyFly_Project.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyFly_Test
{
    internal class FlightOwnerServiceTest
    {
        SimplyFlyDbContext context;
        FlightOwner addedFlightOwner = new FlightOwner();

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<SimplyFlyDbContext>().UseInMemoryDatabase("dummyDatabase").Options;
            context = new SimplyFlyDbContext(options);
        }

        [Order(1)]
        [Test]
        public async Task AddedFlightOwnerTest()
        {
            //Arrange
            var mockFlightOwnerRepositoryLogger = new Mock<ILogger<FlightOwnerRepository>>();
            var mockUserRepositoryLogger = new Mock<ILogger<UserRepository>>();
            var mockFlightOwnerServiceLogger = new Mock<ILogger<FlightOwnerService>>();
            var mockCancelBookingRepositoryLogger = new Mock<ILogger<CancelBookingRepository>>();
            var mockPaymentRepositoryLogger = new Mock<ILogger<PaymentRepository>>();
            var mockBookingRepositoryLogger = new Mock<ILogger<BookingRepository>>();

            IRepository<int, FlightOwner> flightOwnerRepository = new FlightOwnerRepository(context, mockFlightOwnerRepositoryLogger.Object);
            IRepository<string, User> userRepository = new UserRepository(context, mockUserRepositoryLogger.Object);
            IRepository<int, CancelledBooking> cancelBookingRepository = new CancelBookingRepository(context, mockCancelBookingRepositoryLogger.Object);
            IRepository<int, Payment> paymentRepository = new PaymentRepository(context, mockPaymentRepositoryLogger.Object);
            IRepository<int, Booking> bookingRepository = new BookingRepository(context, mockBookingRepositoryLogger.Object);

            IFlightOwnerService flightOwnerService = new FlightOwnerService(flightOwnerRepository, userRepository, cancelBookingRepository, paymentRepository, bookingRepository, mockFlightOwnerServiceLogger.Object);

            User user = new User
            {
                Username = "rovman",
                Password = new byte[] { 0x01, 0x02, 0x03, 0x04 },
                Role = "flightOwner",
                Key = new byte[] { 0xAA, 0xBB, 0xCC, 0xDD }
            };
            await userRepository.Add(user);

            FlightOwner flightOwner = new FlightOwner
            {
                Name = "Rovman King",
                Email = "rovman@gmail.com",
                Username = "rovman"
            };

            //Act 
            addedFlightOwner = await flightOwnerService.AddFlightOwner(flightOwner);

            //Assert
            Assert.That(addedFlightOwner.FlightOwnerId, Is.EqualTo(flightOwner.FlightOwnerId));

        }

        [Order(2)]
        [Test]
        public async Task GetAllFlightOwnersTest()
        {
           
            //Arrange
            var mockFlightOwnerRepositoryLogger = new Mock<ILogger<FlightOwnerRepository>>();
            var mockUserRepositoryLogger = new Mock<ILogger<UserRepository>>();
            var mockFlightOwnerServiceLogger = new Mock<ILogger<FlightOwnerService>>();
            var mockCancelBookingRepositoryLogger = new Mock<ILogger<CancelBookingRepository>>();
            var mockPaymentRepositoryLogger = new Mock<ILogger<PaymentRepository>>();
            var mockBookingRepositoryLogger = new Mock<ILogger<BookingRepository>>();

            IRepository<int, FlightOwner> flightOwnerRepository = new FlightOwnerRepository(context, mockFlightOwnerRepositoryLogger.Object);
            IRepository<string, User> userRepository = new UserRepository(context, mockUserRepositoryLogger.Object);
            IRepository<int, CancelledBooking> cancelBookingRepository = new CancelBookingRepository(context, mockCancelBookingRepositoryLogger.Object);
            IRepository<int, Payment> paymentRepository = new PaymentRepository(context, mockPaymentRepositoryLogger.Object);
            IRepository<int, Booking> bookingRepository = new BookingRepository(context, mockBookingRepositoryLogger.Object);

            IFlightOwnerService flightOwnerService = new FlightOwnerService(flightOwnerRepository, userRepository, cancelBookingRepository, paymentRepository, bookingRepository, mockFlightOwnerServiceLogger.Object);

            var flightOwners = await flightOwnerService.GetAllFlightOwners();

            //Assert
            Assert.IsNotEmpty(flightOwners);
        }


        [Order(3)]
        [Test]
        public async Task RemoveFlightOwnerTest()
        {
            //Arrange
            var mockFlightOwnerRepositoryLogger = new Mock<ILogger<FlightOwnerRepository>>();
            var mockUserRepositoryLogger = new Mock<ILogger<UserRepository>>();
            var mockFlightOwnerServiceLogger = new Mock<ILogger<FlightOwnerService>>();
            var mockCancelBookingRepositoryLogger = new Mock<ILogger<CancelBookingRepository>>();
            var mockPaymentRepositoryLogger = new Mock<ILogger<PaymentRepository>>();
            var mockBookingRepositoryLogger = new Mock<ILogger<BookingRepository>>();

            IRepository<int, FlightOwner> flightOwnerRepository = new FlightOwnerRepository(context, mockFlightOwnerRepositoryLogger.Object);
            IRepository<string, User> userRepository = new UserRepository(context, mockUserRepositoryLogger.Object);
            IRepository<int, CancelledBooking> cancelBookingRepository = new CancelBookingRepository(context, mockCancelBookingRepositoryLogger.Object);
            IRepository<int, Payment> paymentRepository = new PaymentRepository(context, mockPaymentRepositoryLogger.Object);
            IRepository<int, Booking> bookingRepository = new BookingRepository(context, mockBookingRepositoryLogger.Object);

            IFlightOwnerService flightOwnerService = new FlightOwnerService(flightOwnerRepository, userRepository, cancelBookingRepository, paymentRepository, bookingRepository, mockFlightOwnerServiceLogger.Object);
            //Act 
            var deletedFlightOwner = await flightOwnerService.RemoveFlightOwner(1);

            //Assert
            Assert.That(true, Is.EqualTo(deletedFlightOwner));

        }

        [Order(4)]
        [Test]

        public async Task NoSuchFlightOwnerExceptionTest()
        {
            //Arrange
            var mockFlightOwnerRepositoryLogger = new Mock<ILogger<FlightOwnerRepository>>();
            var mockUserRepositoryLogger = new Mock<ILogger<UserRepository>>();
            var mockFlightOwnerServiceLogger = new Mock<ILogger<FlightOwnerService>>();
            var mockCancelBookingRepositoryLogger = new Mock<ILogger<CancelBookingRepository>>();
            var mockPaymentRepositoryLogger = new Mock<ILogger<PaymentRepository>>();
            var mockBookingRepositoryLogger = new Mock<ILogger<BookingRepository>>();

            IRepository<int, FlightOwner> flightOwnerRepository = new FlightOwnerRepository(context, mockFlightOwnerRepositoryLogger.Object);
            IRepository<string, User> userRepository = new UserRepository(context, mockUserRepositoryLogger.Object);
            IRepository<int, CancelledBooking> cancelBookingRepository = new CancelBookingRepository(context, mockCancelBookingRepositoryLogger.Object);
            IRepository<int, Payment> paymentRepository = new PaymentRepository(context, mockPaymentRepositoryLogger.Object);
            IRepository<int, Booking> bookingRepository = new BookingRepository(context, mockBookingRepositoryLogger.Object);

            IFlightOwnerService flightOwnerService = new FlightOwnerService(flightOwnerRepository, userRepository, cancelBookingRepository, paymentRepository, bookingRepository, mockFlightOwnerServiceLogger.Object);

            Assert.ThrowsAsync<NoSuchFlightOwnerException> (async () => await flightOwnerService.GetFlightOwnerById(6969));

        }


        [Test]

        public async Task GetFlightOwnerByUsernameTest()
        {
            //Arrange
            var mockFlightOwnerRepositoryLogger = new Mock<ILogger<FlightOwnerRepository>>();
            var mockUserRepositoryLogger = new Mock<ILogger<UserRepository>>();
            var mockFlightOwnerServiceLogger = new Mock<ILogger<FlightOwnerService>>();
            var mockCancelBookingRepositoryLogger = new Mock<ILogger<CancelBookingRepository>>();
            var mockPaymentRepositoryLogger = new Mock<ILogger<PaymentRepository>>();
            var mockBookingRepositoryLogger = new Mock<ILogger<BookingRepository>>();

            IRepository<int, FlightOwner> flightOwnerRepository = new FlightOwnerRepository(context, mockFlightOwnerRepositoryLogger.Object);
            IRepository<string, User> userRepository = new UserRepository(context, mockUserRepositoryLogger.Object);
            IRepository<int, CancelledBooking> cancelBookingRepository = new CancelBookingRepository(context, mockCancelBookingRepositoryLogger.Object);
            IRepository<int, Payment> paymentRepository = new PaymentRepository(context, mockPaymentRepositoryLogger.Object);
            IRepository<int, Booking> bookingRepository = new BookingRepository(context, mockBookingRepositoryLogger.Object);

            IFlightOwnerService flightOwnerService = new FlightOwnerService(flightOwnerRepository, userRepository, cancelBookingRepository, paymentRepository, bookingRepository, mockFlightOwnerServiceLogger.Object);

            User user = new User
            {
                Username = "naman_o",
                Password = new byte[] { 0x01, 0x02, 0x03, 0x04 },
                Role = "flightOwner",
                Key = new byte[] { 0xAA, 0xBB, 0xCC, 0xDD }
            };
            await userRepository.Add(user);

            FlightOwner flightOwner = new FlightOwner
            {
                Name = "Naman Ojha",
                Email = "naman@gmail.com",
                Username = "naman_o"
            };

            //Act 
            addedFlightOwner = await flightOwnerService.AddFlightOwner(flightOwner);
            var retrievedFlightOwner = await flightOwnerService.GetFlightOwnerByUsername(user.Username);

            //Assert
            Assert.That(retrievedFlightOwner.Username, Is.EqualTo(user.Username));
        }


        [Test]

        public async Task GetFlightOwnerByIdTest()
        {
            //Arrange
            var mockFlightOwnerRepositoryLogger = new Mock<ILogger<FlightOwnerRepository>>();
            var mockUserRepositoryLogger = new Mock<ILogger<UserRepository>>();
            var mockFlightOwnerServiceLogger = new Mock<ILogger<FlightOwnerService>>();
            var mockCancelBookingRepositoryLogger = new Mock<ILogger<CancelBookingRepository>>();
            var mockPaymentRepositoryLogger = new Mock<ILogger<PaymentRepository>>();
            var mockBookingRepositoryLogger = new Mock<ILogger<BookingRepository>>();

            IRepository<int, FlightOwner> flightOwnerRepository = new FlightOwnerRepository(context, mockFlightOwnerRepositoryLogger.Object);
            IRepository<string, User> userRepository = new UserRepository(context, mockUserRepositoryLogger.Object);
            IRepository<int, CancelledBooking> cancelBookingRepository = new CancelBookingRepository(context, mockCancelBookingRepositoryLogger.Object);
            IRepository<int, Payment> paymentRepository = new PaymentRepository(context, mockPaymentRepositoryLogger.Object);
            IRepository<int, Booking> bookingRepository = new BookingRepository(context, mockBookingRepositoryLogger.Object);

            IFlightOwnerService flightOwnerService = new FlightOwnerService(flightOwnerRepository, userRepository, cancelBookingRepository, paymentRepository, bookingRepository, mockFlightOwnerServiceLogger.Object);

            User user = new User
            {
                Username = "rahul_s",
                Password = new byte[] { 0x01, 0x02, 0x03, 0x04 },
                Role = "flightOwner",
                Key = new byte[] { 0xAA, 0xBB, 0xCC, 0xDD }
            };
            await userRepository.Add(user);

            FlightOwner flightOwner = new FlightOwner
            {
                Name = "Rahul Saha",
                Email = "rahul@gmail.com",
                Username = "rahul_s"
            };

            //Act 
            addedFlightOwner = await flightOwnerService.AddFlightOwner(flightOwner);
            var retrievedFlightOwner = await flightOwnerService.GetFlightOwnerById(addedFlightOwner.FlightOwnerId);

            Assert.That(addedFlightOwner.FlightOwnerId, Is.EqualTo(retrievedFlightOwner.FlightOwnerId));

        }


        [Order(7)]
        [Test]

        public async Task UpdateFlightOwnerTest()
        {
            //Arrange
            var mockFlightOwnerRepositoryLogger = new Mock<ILogger<FlightOwnerRepository>>();
            var mockUserRepositoryLogger = new Mock<ILogger<UserRepository>>();
            var mockFlightOwnerServiceLogger = new Mock<ILogger<FlightOwnerService>>();
            var mockCancelBookingRepositoryLogger = new Mock<ILogger<CancelBookingRepository>>();
            var mockPaymentRepositoryLogger = new Mock<ILogger<PaymentRepository>>();
            var mockBookingRepositoryLogger = new Mock<ILogger<BookingRepository>>();

            IRepository<int, FlightOwner> flightOwnerRepository = new FlightOwnerRepository(context, mockFlightOwnerRepositoryLogger.Object);
            IRepository<string, User> userRepository = new UserRepository(context, mockUserRepositoryLogger.Object);
            IRepository<int, CancelledBooking> cancelBookingRepository = new CancelBookingRepository(context, mockCancelBookingRepositoryLogger.Object);
            IRepository<int, Payment> paymentRepository = new PaymentRepository(context, mockPaymentRepositoryLogger.Object);
            IRepository<int, Booking> bookingRepository = new BookingRepository(context, mockBookingRepositoryLogger.Object);

            IFlightOwnerService flightOwnerService = new FlightOwnerService(flightOwnerRepository, userRepository, cancelBookingRepository, paymentRepository, bookingRepository, mockFlightOwnerServiceLogger.Object);

            User user = new User
            {
                Username = "kiran_",
                Password = new byte[] { 0x01, 0x02, 0x03, 0x04 },
                Role = "flightOwner",
                Key = new byte[] { 0xAA, 0xBB, 0xCC, 0xDD }
            };
            await userRepository.Add(user);

            FlightOwner flightOwner = new FlightOwner
            {
                Name = "Kiran Rao",
                Email = "kiran@gmail.com",
                Username = "kiran_"
            };

            //Act 
            addedFlightOwner = await flightOwnerService.AddFlightOwner(flightOwner);

            UpdateFlightOwnerDTO updateFlightOwner = new UpdateFlightOwnerDTO()
            {
                FlightOwnerId = addedFlightOwner.FlightOwnerId,
                Email = "kiraan_updated@gmail.com"
            };

            var updatedDetails = await flightOwnerService.UpdateFlightOwner(updateFlightOwner);
            Assert.AreEqual(updateFlightOwner.Email, updatedDetails.Email);

        }
    }
}
