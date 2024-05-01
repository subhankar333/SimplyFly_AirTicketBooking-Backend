using SimplyFly_Project.DTO;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using SimplyFly_Project.Repositories;

namespace SimplyFly_Project.Services
{
    public class FlightOwnerService : IFlightOwnerService
    {
        private readonly IRepository<int, FlightOwner> _flightOwnerRepository;
        private readonly IRepository<string, User> _userRepository;
        private readonly IRepository<int, CancelledBooking> _cancelBookingRepository;
        private readonly IRepository<int, Payment> _paymentRepository;
        private readonly IRepository<int, Booking> _bookingRepository;
        private readonly ILogger<FlightOwnerService> _logger;

        public FlightOwnerService(IRepository<int, FlightOwner> flightOwnerRepository, IRepository<string, User> userRepository, IRepository<int, CancelledBooking> cancelBookingRepository, IRepository<int, Payment> paymentRepository, IRepository<int, Booking> bookingRepository, ILogger<FlightOwnerService> logger)
        {
            _flightOwnerRepository = flightOwnerRepository;
            _userRepository = userRepository;
            _cancelBookingRepository = cancelBookingRepository;
            _paymentRepository = paymentRepository;
            _bookingRepository = bookingRepository;
            _logger = logger;
        }
        public async Task<FlightOwner> AddFlightOwner(FlightOwner flightOwner)
        {
            return await _flightOwnerRepository.Add(flightOwner);
        }

        public async Task<List<FlightOwner>> GetAllFlightOwners()
        {
            return await _flightOwnerRepository.GetAsync();
        }

        public async Task<FlightOwner> GetFlightOwnerById(int flightOwnerId)
        {
            var flightOwners = await _flightOwnerRepository.GetAsync();
            var flightOwner = flightOwners.FirstOrDefault(e => e.FlightOwnerId == flightOwnerId);

            if (flightOwner != null)
            {
                return flightOwner;
            }

            throw new NoSuchFlightOwnerException();
        }

        public async Task<FlightOwner> GetFlightOwnerByUsername(string userName)
        {
            var flightOwners = await _flightOwnerRepository.GetAsync();
            var flightOwner = flightOwners.FirstOrDefault(fo => fo.Username == userName);
            if (flightOwner != null)
            {
                return flightOwner;
            }

            throw new NoSuchFlightOwnerException();
        }

        public async Task<bool> RemoveFlightOwner(int flightOwnerId)
        {
            var owner = await _flightOwnerRepository.GetAsync(flightOwnerId);
            if (owner != null)
            {
                await _flightOwnerRepository.Delete(flightOwnerId);
                await _userRepository.Delete(owner.Username);

                return true;
            }
            return false;
        }

        public async Task<FlightOwner> UpdateFlightOwner(UpdateFlightOwnerDTO flightOwner)
        {
            var _flightowner = await _flightOwnerRepository.GetAsync(flightOwner.FlightOwnerId);
            if(_flightowner != null)
            {
                _flightowner.Name = flightOwner.Name;
                _flightowner.Email = flightOwner.Email;
                _flightowner.ContactNumber = flightOwner.ContactNumber;
                _flightowner.CompanyName = flightOwner.CompanyName;
                _flightowner.Address = flightOwner.Address;
                _flightowner = await _flightOwnerRepository.Update(_flightowner);
                return _flightowner;
            }

            throw new NoSuchFlightOwnerException();
        }

        public async Task<CancelledBooking> UpdateRefundStatus(int cancelBookingId,string status)
        {
            var cancelledBooking = await _cancelBookingRepository.GetAsync(cancelBookingId);
            if(cancelledBooking != null && status != "---Select---")
            {
                cancelledBooking.RefundStatus = status;
                cancelledBooking = await _cancelBookingRepository.Update(cancelledBooking);

                return cancelledBooking;
            }

            throw new NoSuchBookingsException();
        }
    }
}
