using SimplyFly_Project.Models;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.DTO
{
    [ExcludeFromCodeCoverage]
    public class BookingRequestDTO
    {
        public int ScheduleId { get; set; }
        public int CustomerId { get; set; }
        public DateTime BookingTime { get; set; }
        public List<int> PassengerIds { get; set; }
        public List<string> SelectedSeats { get; set; }
        public double Price { get; set; }
        public PaymentDetails PaymentDetails { get; set; }
    }
}
