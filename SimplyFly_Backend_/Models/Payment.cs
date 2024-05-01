using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;

namespace SimplyFly_Project.Models
{
    [ExcludeFromCodeCoverage]
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
        public PaymentDetails PaymentDetails { get; set; } = new PaymentDetails();



        public Payment()
        {
            PaymentId = 0;
        }

        public Payment(int paymentId, double amount, DateTime paymentDate, string paymentStatus, PaymentDetails paymentDetails, Booking booking)
        {
            PaymentId = paymentId;
            Amount = amount;
            PaymentDate = paymentDate;
            PaymentStatus = paymentStatus;
            PaymentDetails = paymentDetails;

        }
        public Payment(double amount, DateTime paymentDate, string paymentStatus, PaymentDetails paymentDetails, Booking booking)
        {
            Amount = amount;
            PaymentDate = paymentDate;
            PaymentStatus = paymentStatus;
            PaymentDetails = paymentDetails;

        }

    }


    public class PaymentDetails
    {
        public int PaymentDetailsId { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public string ExpiryDate { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;

    }
}
