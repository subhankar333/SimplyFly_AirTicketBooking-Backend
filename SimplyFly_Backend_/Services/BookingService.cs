using SimplyFly_Project.DTO;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;

namespace SimplyFly_Project.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<int, Booking> _bookingRepository;
        private readonly IRepository<string, Flight> _flightRepository;
        private readonly IRepository<int, PassengerBooking> _passengerBookingRepository;
        private readonly IRepository<int,Schedule> _scheduleRepository;
        private readonly IRepository<int,Payment> _paymentRepository;
        private readonly IRepository<string,SeatDetail> _seatRepository;
        private readonly IRepository<int,CancelledBooking> _cancelBookingRepository;

        private readonly ISeatDetailRepository _seatDetailRepository;
        private readonly IPassengerBookingRepository _passengerBookingsRepository;
        private readonly IBookingRepository _bookingsRepository;

        private readonly ILogger<BookingService> _logger;

        public BookingService(IRepository<int, Booking> bookingRepository, IRepository<string, Flight> flightRepository, IRepository<int, PassengerBooking> passengerBookingRepository, IRepository<int, Schedule> scheduleRepository, IRepository<int, Payment> paymentRepository, ISeatDetailRepository seatDetailRepository, IPassengerBookingRepository passengerBookingsRepository, IBookingRepository bookingsRepository, IRepository<string, SeatDetail> seatRepository, IRepository<int, CancelledBooking> cancelBookingRepository, ILogger<BookingService> logger)
        {
            _bookingRepository = bookingRepository;
            _flightRepository = flightRepository;
            _passengerBookingRepository = passengerBookingRepository;
            _scheduleRepository = scheduleRepository;
            _paymentRepository = paymentRepository;
            _seatDetailRepository = seatDetailRepository;
            _passengerBookingsRepository = passengerBookingsRepository;
            _bookingsRepository = bookingsRepository;
            _seatRepository = seatRepository;
            _cancelBookingRepository = cancelBookingRepository;
            _logger = logger;
        }


        public async Task<Booking> CancelBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetAsync(bookingId);

            if (booking.ScheduleId == -1)
            {
                throw new NoSuchBookingsException();
            }

            Booking? bookingsList = await _bookingRepository.GetAsync(bookingId);
            List<SeatDetail>? seatsList = await _seatRepository.GetAsync();
            Schedule? _schedule = await _scheduleRepository.GetAsync(bookingsList.ScheduleId);
            List<PassengerBooking>? passBookings = await _passengerBookingRepository.GetAsync();
            string? flightNo = "";

            if (_schedule != null)
            {
                flightNo = _schedule.FlightNumber;
            }

            CancelledBooking _cancelBooking = new CancelledBooking();
            _cancelBooking.BookingId = bookingId;
            _cancelBooking.Booking = bookingsList;
            _cancelBooking.RefundStatus = "Refund Requested";
            await _cancelBookingRepository.Add(_cancelBooking);
            



            if (passBookings != null && flightNo != null)
            {
                foreach (var item in passBookings)
                {
                    if (seatsList != null)
                    {
                        foreach (var seat in seatsList)
                        {
                            if (seat.FlightNumber == flightNo && seat.SeatNo == item.SeatNumber)
                            {
                                seat.isBooked = 0;
                            }
                        }
                    }
                }
            }

            // Delete passenger booking
            var passengerBookings = await _passengerBookingsRepository.GetPassengerBookingAsync(bookingId);
            foreach (var passengerBooking in passengerBookings)
            {
                await _passengerBookingRepository.Delete(passengerBooking.PassengerBookingId);
            }

            // Delete payment
            await _paymentRepository.Delete(booking.PaymentId);
 

            // Delete booking(paymentId is FK, so auto delete happening)
            // return await _bookingRepository.Delete(booking.BookingId);

            return booking;
        }

        public async Task<PassengerBooking> CancelBookingByPassenger(int passengerId)
        {
            var passengerBookings = await _passengerBookingRepository.GetAsync();
            PassengerBooking? passengerBooking = passengerBookings?.FirstOrDefault(pb => pb.PassengerId == passengerId);

            if (passengerBooking != null)
            {
                List<SeatDetail>? seatsList = await _seatRepository.GetAsync();
                PassengerBooking? passBookings = await _passengerBookingRepository.GetAsync(passengerBooking.PassengerBookingId);
                List<Booking>? _bookingsList = await _bookingRepository.GetAsync();

                Booking? booking = _bookingsList.FirstOrDefault(b => b.BookingId == passBookings.BookingId);
                Schedule? _schedule = await _scheduleRepository.GetAsync(booking.ScheduleId);

                //adding to cancelBooking
                CancelledBooking _cancelBooking = new CancelledBooking();
                _cancelBooking.BookingId = (int)passengerBooking.BookingId;
                _cancelBooking.Booking = booking;
                _cancelBooking.RefundStatus = "Pending";
                _cancelBooking =  await _cancelBookingRepository.Add(_cancelBooking);

                //updating booking status
                booking.BookingStatus = "Cancelled";
                await _bookingRepository.Update(booking);

                //refund request
                RequestRefundAsync(booking.BookingId, _cancelBooking.CancelId);


                var flightNo = _schedule.FlightNumber;

                if (passBookings != null)
                {
                    if (seatsList != null)
                    {
                        foreach (var seat in seatsList)
                        {
                            if (seat.FlightNumber == flightNo && seat.SeatNo == passBookings.SeatNumber)
                            {
                                seat.isBooked = 0;
                            }
                        }
                    }
                }

                passengerBooking = await _passengerBookingRepository.Delete(passengerBooking.PassengerBookingId);
                return passengerBooking;
            }

            throw new NoSuchPassengerException();
        }

        public async Task<bool> CreateBookingAsync(BookingRequestDTO bookingRequest)
        {
            if(bookingRequest == null)
            {
                throw new ArgumentNullException(nameof(bookingRequest));
            }

            List<SeatDetail>? seatsList = await _seatRepository.GetAsync();
            var _schedule = await _scheduleRepository.GetAsync(bookingRequest.ScheduleId);

            var flightNo = _schedule.FlightNumber;

            var isSeatsAvailable = await _passengerBookingsRepository.CheckSeatsAvailbilityAsync(bookingRequest.ScheduleId, bookingRequest.SelectedSeats);
            //If selected seats are not available 
            if(!isSeatsAvailable)
            {
                return false;
            }

            int count = 0;

            if(seatsList != null)
            {
                foreach (var seat in bookingRequest.SelectedSeats)
                {
                    foreach (var item in seatsList)
                    {
                        if (item.FlightNumber == flightNo && item.SeatNo == seat && item.isBooked == 1)
                        {
                            throw new Exception("Invalid seat selection, please try selecting correctly.");
                        }
                        else if (item.FlightNumber == flightNo && item.SeatNo == seat && item.isBooked == 0)
                        {
                            count++;
                        }
                    }
                }
            }

            if(count != bookingRequest.SelectedSeats.Count)
            {
                throw new Exception("Invalid seat selection, please try selecting correctly.");
            }
           

            var schedudle = await _scheduleRepository.GetAsync(bookingRequest.ScheduleId);
            if(schedudle == null)
            {
                throw new NoSuchScheduleException();
            }

            //Calculate total prices based on booked tickets
            var totalPrice = CalculateTotalPrice(bookingRequest.SelectedSeats.Count, await _flightRepository.GetAsync(schedudle.FlightNumber));
            //Getting the seatClass type 
            var seatClass = bookingRequest.SelectedSeats[0][0];

            //Creating payment
            var payment = new Payment
            {
                Amount = bookingRequest.Price,
                PaymentDate = DateTime.Now, 
                PaymentStatus = "Successful",
                PaymentDetails = bookingRequest.PaymentDetails,
            };
            var addedPayment = await _paymentRepository.Add(payment);

            //Creating booking
            var booking = new Booking
            {
                ScheduleId = bookingRequest.ScheduleId,
                CustomerId = bookingRequest.CustomerId,
                BookingTime = DateTime.Now,
                TotalPrice = bookingRequest.Price,
                BookingStatus = "Booked",
                PaymentId = addedPayment.PaymentId,
            };

            
            var addedBooking = await _bookingRepository.Add(booking);

            //fetching the seats only for given seatNos
            var seatDetails = await _seatDetailRepository.GetSeatDetailsAsync(bookingRequest.SelectedSeats);

            if(seatDetails == null || seatDetails.Count() != bookingRequest.SelectedSeats.Count())
            {
                throw new Exception("Invalid seat selection, please try selecting correctly.");
            }



            //creating PassengerBooking, assigning seatNumbers
            int index = 0;
            foreach(var passengerId in bookingRequest.PassengerIds)
            {
                //get seatDetail for current index
                var seatDetail = seatDetails.ElementAtOrDefault(index);
                if(seatDetail != null)
                {
                    var passengerBooking = new PassengerBooking
                    {
                        BookingId = addedBooking.BookingId,
                        PassengerId = passengerId,
                        SeatNumber = seatDetail.SeatNo // Assign a unique seat to each passenger
                    };

                    if(seatsList != null)
                    {
                        foreach (var item in seatsList)
                        {
                            if (item.FlightNumber == flightNo && item.SeatNo == seatDetail.SeatNo)
                            {
                                item.isBooked = 1;
                            }
                        }
                    }

                    await _passengerBookingRepository.Add(passengerBooking);
                    // Move to the next seat for the next passenger
                    index++;
                }

                //when no of seats < no of passengers
                else
                {
                    throw new Exception("Not enough seats available for the passengers");
                }
            }

            return addedBooking != null && addedPayment != null;
        }


        public double CalculateTotalPrice(int numberOfSeats, Flight flight)
        {
            double totalPrice = numberOfSeats * (flight?.BasePrice ?? 0);
            return totalPrice;
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAsync();
            List<Booking> bookingList = new List<Booking>();

            foreach (var item in bookings)
            {
                if (item.BookingStatus == "Booked")
                    bookingList.Add(item);
            }

            return bookingList;
        }

        public async Task<List<string>> GetBookedSeatBySchedule(int scheduleID)
        {
            var bookings = await _passengerBookingRepository.GetAsync();
            var bookedSeats = bookings?.Where(e => e.Booking.ScheduleId == scheduleID)
                .Select(e => e.SeatNumber).ToList();

            if (bookedSeats != null)
            {
                return bookedSeats;
            }

            throw new NoSuchBookingsException();
        }

        public async Task<List<Booking>> GetBookingByFlight(string flightNumber)
        {
            var bookings = await _bookingRepository.GetAsync();
            bookings = bookings.Where(e => e.Schedule.FlightNumber == flightNumber).ToList();

            if (bookings != null)
            {
                return bookings;
            }

            throw new NoSuchBookingsException();
        }

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            return await _bookingRepository.GetAsync(bookingId);
        }

        public async Task<List<PassengerBooking>> GetBookingsByCustomerId(int customerId)
        {
            var bookings = await _passengerBookingRepository.GetAsync();
            bookings = bookings.Where(e => e.Booking.CustomerId == customerId).ToList();

            if (bookings != null)
            {
                return bookings;
            }

            throw new NoSuchCustomerException();
        }

        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(int customerId)
        {
            var bookings = await _bookingsRepository.GetBookingsByCustomerIdAsync(customerId);

            if (bookings != null)
            {
                //bookings = bookings.Where(e => e.Schedule.Departure < DateTime.Now);
                return bookings;
            }

            throw new NoSuchBookingsException();

            //return await _bookingsRepository.GetBookingsByCustomerIdAsync(customerId);
        }

        public async Task<bool> RequestRefundAsync(int bookingId, int cancelBookingId)
        {
            Booking? booking = await _bookingRepository.GetAsync(bookingId);
            List<CancelledBooking>? cancelBookings = await _cancelBookingRepository.GetAsync();
            CancelledBooking? cancelBooking = cancelBookings?.FirstOrDefault(cb => cb.BookingId == bookingId && cb.CancelId == cancelBookingId);


            if (booking == null)
            {
                throw new NoSuchBookingsException();
            }

            // Check if payment exists
            var payment = await _paymentRepository.GetAsync(booking.PaymentId);

            if (payment == null)
            {
                throw new Exception("Payment not found for the booking.");
            }

            // Check payment status
            if (payment.PaymentStatus != "Successful")
            {
                throw new Exception("Refund cannot be requested for unsuccessful payments.");
            }

            // Updating paymentStatus to "RefundRequested" for refund request
            if(payment != null)
            {
                payment.PaymentStatus = "RefundRequested";
            }
            if(cancelBooking != null)
            {
                cancelBooking.RefundStatus = payment.PaymentStatus;
            }
           

            await _paymentRepository.Update(payment);
            await _cancelBookingRepository.Update(cancelBooking);

            return true;
        }

        public async Task<List<Booking>> GetBookingBySchedule(int scheduleId)
        {
            var bookings = await _bookingRepository.GetAsync();
            bookings = bookings?.Where(b => b.ScheduleId ==  scheduleId).ToList();

            if (bookings != null)
            {
                return bookings;
            }

            throw new NoSuchBookingsException();
        }

        public async Task<List<CancelledBooking>> GetAllCancelledBookings()
        {
            var cancelledBookings = await _cancelBookingRepository.GetAsync();
            return cancelledBookings;
        }
    }
}
