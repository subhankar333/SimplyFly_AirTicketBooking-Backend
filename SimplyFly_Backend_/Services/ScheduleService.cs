using SimplyFly_Project.DTO;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;

namespace SimplyFly_Project.Services
{
    public class ScheduleService : IScheduleFlightOwnerService, IFlightCustomerService
    {
        private readonly IRepository<int, Schedule> _scheduleRepository;
        public readonly IRepository<int, Passenger> _passengerRepository;
        private readonly IRepository<string,Flight> _flightRepository;
        private readonly IBookingService _bookingService;
        private readonly ISeatDetailService _seatDetailService;
        private readonly ILogger<ScheduleService> _logger;


        public ScheduleService(IRepository<int, Schedule> scheduleRepository, ILogger<ScheduleService> logger)
        {
            _scheduleRepository = scheduleRepository;
            _logger = logger;

        }
        public ScheduleService(IRepository<int, Schedule> scheduleRepository, IBookingService bookingService, ILogger<ScheduleService> logger, ISeatDetailService seatDetailService, IRepository<int, Passenger> passengerRepository,IRepository<string,Flight> flightRepository)
        {
            _scheduleRepository = scheduleRepository;
            _bookingService = bookingService;
            _logger = logger;
            _seatDetailService = seatDetailService;
            _passengerRepository = passengerRepository;
            _flightRepository = flightRepository;
        }
        public async Task<Schedule> AddSchedule(Schedule schedule)
        {
            var existingSchedules = await _scheduleRepository.GetAsync();

            //checking conditions if flights overlapping
            bool isOverlap = existingSchedules.Any(e => e.FlightNumber == schedule.FlightNumber &&
            ((schedule.Departure >= e.Departure && schedule.Departure <= e.Arrival) ||
            (schedule.Arrival >= e.Departure && schedule.Arrival <= e.Arrival) ||
            (e.Departure >= schedule.Departure && e.Arrival <= schedule.Arrival)));

            if (!isOverlap)
            {
                // If no overlap exists then only adding the new schedule
                schedule = await _scheduleRepository.Add(schedule);
                return schedule;
            }

            throw new FlightScheduleBusyException();
        }

        public async Task<List<Schedule>> GetAllSchedules()
        {
            var schedules = await _scheduleRepository.GetAsync();
            return schedules;
        }

        public async Task<List<FlightScheduleDTO>> GetFlightSchedule(string flightNumber)
        {
            List<FlightScheduleDTO> flightSchedule = new List<FlightScheduleDTO>();

            var schedules = await _scheduleRepository.GetAsync();
            schedules = schedules.Where(e => e.FlightNumber == flightNumber).ToList();

            flightSchedule = schedules.Select(e => new FlightScheduleDTO
            {
                FlightNumber = e.FlightNumber,
                SourceAirport = e.Route?.SourceAirport?.Name + " ," + e.Route?.SourceAirport?.City,
                DestinationAirport = e.Route?.DestinationAirport?.Name + " ," + e.Route?.DestinationAirport?.City,
                Departure = e.Departure,
                Arrival = e.Arrival
            }).ToList();

            if (flightSchedule != null)
                return flightSchedule;

            else
                throw new NoSuchScheduleException();
        }

        public async Task<Schedule> RemoveSchedule(Schedule schedule)
        {
            var schedules = await _scheduleRepository.GetAsync(schedule.ScheduleId);
            if (schedules != null)
            {
                schedules = await _scheduleRepository.Delete(schedules.ScheduleId);
                return schedules;
            }
            throw new NoSuchScheduleException();
        }

        public async Task<int> RemoveSchedule(string flightNo)
        {
            int removedScheduledCount = 0;
            var schedules = await _scheduleRepository.GetAsync();
            schedules = schedules.Where(s => s.FlightNumber == flightNo).ToList();

            foreach(var flight in schedules)
            {
                await _scheduleRepository.Delete(flight.ScheduleId);
                removedScheduledCount++;
            }

            if (removedScheduledCount > 0)
                return removedScheduledCount;

            throw new NoSuchScheduleException();

        }

        public async Task<int> RemoveSchedule(DateTime departureDate, int airportId)
        {
            int removedScheduleCount = 0;
            var schedules = await _scheduleRepository.GetAsync();
            schedules = schedules.Where(s => s.Departure.Date == departureDate.Date && s.Route.SourceAirportId == airportId).ToList();

            foreach (var flight in schedules)
            {
                await _scheduleRepository.Delete(flight.ScheduleId);
                removedScheduleCount++;
            }

            if (removedScheduleCount > 0)
                return removedScheduleCount;

            throw new NoSuchScheduleException();
        }


        public int AvailableSeats(int totalSeats, int scheduleId)
        {
            var bookedSeatsTask = _bookingService.GetBookedSeatBySchedule(scheduleId);
            bookedSeatsTask.Wait();
            var bookedSeats = bookedSeatsTask.Result;

            int availableSeats;

            if (bookedSeats == null)
                availableSeats = totalSeats;
            else
                availableSeats = totalSeats - bookedSeats.Count();

            return availableSeats;
        }

        public double CalculateTotalPrice(SearchFlightDTO searchFlight, double basePrice)
        {
            double totalPrice = 0;
            double seatPrice = 0;
            double adultSeatCost = 0;
            double childSeatCost = 0;

            if (searchFlight.SeatClass == "Economy")
                seatPrice = basePrice * 0.2;

            else if (searchFlight.SeatClass == "PremiumEconomy")
                seatPrice = basePrice * 0.3;

            else
                seatPrice = basePrice * 0.4;

            adultSeatCost = basePrice + seatPrice + (basePrice * 0.3);
            childSeatCost = basePrice + seatPrice + (basePrice * 0.2);

            totalPrice = (adultSeatCost * searchFlight.Adult) + (childSeatCost * searchFlight.Child);

            return totalPrice;
        }

        public async Task<List<SearchedFlightResultDTO>> SearchFlights(SearchFlightDTO searchFlight)
        {
            List<SearchedFlightResultDTO> searchResult = new List<SearchedFlightResultDTO>();
            var schedules = await _scheduleRepository.GetAsync();

             schedules = schedules.Where(e => e.Departure.Date == searchFlight.DateOfJourney.Date
             && e.Route?.SourceAirport?.City == searchFlight.Origin
             && e.Route.DestinationAirport?.City == searchFlight.Destination
             && (AvailableSeats(e.Flight.TotalSeats, e.ScheduleId) > 0)).ToList();

            searchResult = schedules.Select(e => new SearchedFlightResultDTO
            {
                FlightNumber = e.FlightNumber,
                Airline = e.Flight.Airline,
                ScheduleId = e.ScheduleId,
                SourceAirport = e.Route.SourceAirport.City,
                DestinationAirport = e.Route.DestinationAirport.City,
                DepartureTime = e.Departure,
                ArrivalTime = e.Arrival,
                TotalPrice = CalculateTotalPrice(searchFlight, e.Flight.BasePrice)

            }).ToList();


            if (searchResult != null)
                return searchResult;

            else
                throw new NoFlightAvailableException();
        }

        public async Task<Schedule> UpdateScheduledFlight(int scheduleId, string flightNumber)
        {
            var schedule = await _scheduleRepository.GetAsync(scheduleId);
            if (schedule != null && flightNumber[0] == 'F')
            {
                schedule.FlightNumber = flightNumber;

                schedule = await _scheduleRepository.Update(schedule);
                return schedule;
            }
            throw new NoSuchScheduleException();
        }

        public async Task<Schedule> UpdateScheduledRoute(int scheduleId, int routeId)
        {
            var schedule = await _scheduleRepository.GetAsync(scheduleId);
            if (schedule != null)
            {
                schedule.RouteId = routeId;
                schedule = await _scheduleRepository.Update(schedule);
                return schedule;
            }
            throw new NoSuchScheduleException();
        }

        public async Task<Schedule> UpdateScheduledTime(int scheduleId, DateTime departure, DateTime arrival)
        {
            var schedule = await _scheduleRepository.GetAsync(scheduleId);
            if (schedule != null)
            {
                schedule.Departure = departure;
                schedule.Arrival = arrival;
                schedule = await _scheduleRepository.Update(schedule);
                return schedule;
            }
            throw new NoSuchScheduleException();
        }

        public async Task<List<string>> GetAvailableSeatsByFlightNo(string flightNo)
        {
            var seatsDetails = await _seatDetailService.GetAllSeatDetails();
            seatsDetails = seatsDetails.Where(s => s.FlightNumber == flightNo).ToList();

            if(seatsDetails.Count == 0)
            {
                throw new NoSuchFlightException();
            }

            List<string> seatNos = new List<string>();

            int availableSeats = 0;
            foreach (var seat in seatsDetails)
            {
                if (seat.isBooked == 0)
                    seatNos.Add(seat.SeatNo);
                    
            }

            return seatNos;
        }



        public async Task<double> CalculatePrice(string flightNo, List<string> seatNos, List<int> passengerIds)
        {
            var seatsDetails = await _seatDetailService.GetAllSeatDetails();
            var seatDetail = seatsDetails.Where(sd => sd.FlightNumber == flightNo).ToList();

            if (seatsDetails.Count == 0)
            {
                throw new NoSuchFlightException();
            }

            int count = seatDetail.Count(seatDetail => seatNos.Contains(seatDetail.SeatNo));
            if(count != passengerIds.Count || count != seatNos.Count || seatNos.Count != passengerIds.Count)
            {
                throw new InvalidSelectionWithFlight();
            }

            //getting basePrice of flight
            var flight = await _flightRepository.GetAsync(flightNo);
            var basePrice = flight.BasePrice;


            //calculating no of adult and child
            int adult = 0, child = 0;
            var passengers = await _passengerRepository.GetAsync();
            foreach (var id in passengerIds)
            {
                var _passenger = await _passengerRepository.GetAsync(id);
                if (_passenger.Age <= 16)
                    child++;
                else
                    adult++;
            }

            double totalPrice = 0;
            double seatPrice = 0;
            double adultSeatCost = 0;
            double childSeatCost = 0;



            foreach (var item in seatNos)
            {
                var _seatDetail = seatsDetails.FirstOrDefault(sd => sd.SeatNo == item);
                var seatClassType = _seatDetail.SeatClass;

                if (seatClassType == "Economy")
                    seatPrice = basePrice * 0.2;

                else if (seatClassType == "PremiumEconomy")
                    seatPrice = basePrice * 0.3;

                else
                    seatPrice = basePrice * 0.4;

            }


            adultSeatCost = basePrice + seatPrice + (basePrice * 0.3);
            childSeatCost = basePrice + seatPrice + (basePrice * 0.2);

            totalPrice = (adultSeatCost * adult) + (childSeatCost * child);

            return totalPrice;

        }
    }
}
