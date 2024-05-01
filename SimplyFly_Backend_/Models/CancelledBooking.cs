using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Models
{
    [ExcludeFromCodeCoverage]
    public class CancelledBooking
    {
        [Key]
        public int CancelId { get; set; }
        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public Booking? Booking { get; set; }
        public string RefundStatus { get; set; } = string.Empty;

        public CancelledBooking()
        {

        }
        public CancelledBooking(int cancelId, int bookingId, Booking? booking, string refundStatus)
        {
            CancelId = cancelId;
            BookingId = bookingId;
            Booking = booking;
            RefundStatus = refundStatus;
        }
    }
}
