using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Models
{
    [ExcludeFromCodeCoverage]
    public class SeatDetail:IEquatable<SeatDetail>
    {
        [Key]
        public string SeatNo { get; set; }
        public string SeatClass { get; set; }
        public string? FlightNumber { get; set; }
        [ForeignKey("FlightNumber")]
        public Flight? Flight { get; set; }
        public int isBooked { get; set; } = 0;

        public SeatDetail()
        {

        }

        public SeatDetail(string seatNumber, string seatClass)
        {
            SeatNo = seatNumber;
            SeatClass = seatClass;
        }

        public bool Equals(SeatDetail? other)
        {
            var seatDetail = other ?? new SeatDetail();
            return this.SeatNo.Equals(seatDetail.SeatNo);
        }

    }
}
