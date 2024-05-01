using SimplyFly_Project.DTO;

namespace SimplyFly_Project.Interfaces
{
    public interface IFlightCustomerService
    {
        public Task<List<SearchedFlightResultDTO>> SearchFlights(SearchFlightDTO searchFlight);
        public Task<List<string>> GetAvailableSeatsByFlightNo(string flightNo);
        public Task<double> CalculatePrice(string flightNo, List<string> seats, List<int> passengerIds);
        
    }
}
