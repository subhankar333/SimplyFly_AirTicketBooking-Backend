using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Models
{
    [ExcludeFromCodeCoverage]
    public class PassengerBooking
    {
        [Key]
        public int PassengerBookingId { get; set; }
        public int? BookingId { get; set; }

        [ForeignKey("BookingId")]
        public Booking? Booking { get; set; }

        public int? PassengerId { get; set; }

        [ForeignKey("PassengerId")]
        public Passenger? Passenger { get; set; }

        public string SeatNumber { get; set; }

        [ForeignKey("SeatNumber")]
        public SeatDetail? SeatDetail { get; set; }


        public PassengerBooking()
        {
            
        }
        public PassengerBooking(int id)
        {
            PassengerBookingId = id;

        }

        public PassengerBooking(int id, int? bookingId, int? passengerId, string? seatId) : this(id)
        {
            BookingId = bookingId;
            PassengerId = passengerId;
            SeatNumber = seatId;
        }
    }
}
