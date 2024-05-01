using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static SimplyFly_Project.Models.Booking;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Models
{
    [ExcludeFromCodeCoverage]
    public class Booking:IEquatable<Booking>
    {
        [Key]
        public int BookingId { get; set; }
        public int ScheduleId { get; set; }
  
        [ForeignKey("ScheduleId")]
        public Schedule? Schedule { get; set; }

        public int PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public Payment? Payment { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public DateTime BookingTime { get; set; }
        public string BookingStatus { get; set; }
        public double TotalPrice { get; set; }

        public Booking()
        {

        }

        public Booking(int scheduleId, int paymentid, int customerId, DateTime bookingTime, double totalPrice, string bookingstatus)
        {
            ScheduleId = scheduleId;
            CustomerId = customerId;
            BookingTime = bookingTime;
            TotalPrice = totalPrice;
            PaymentId = paymentid;
            BookingStatus = bookingstatus;

        }

        public Booking(int bookingId, int scheduleId, int paymentid, int customerId, DateTime bookingTime, double totalPrice, string bookingstatus)
        {
            BookingId = bookingId;
            ScheduleId = scheduleId;
            CustomerId = customerId;
            BookingTime = bookingTime;
            TotalPrice = totalPrice;
            PaymentId = paymentid;
            BookingStatus = bookingstatus;
        }

        public bool Equals(Booking? other)
        {
           var booking = other ?? new Booking();
            return this.BookingId.Equals(booking.BookingId);
        }
    }
}

