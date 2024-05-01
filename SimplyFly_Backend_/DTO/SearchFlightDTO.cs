using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.DTO
{
    [ExcludeFromCodeCoverage]
    public class SearchFlightDTO
    {
        public DateTime DateOfJourney { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int Adult { get; set; }
        public int Child { get; set; }
        public string SeatClass { get; set; }
    }
}
